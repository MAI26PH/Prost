using EntityFramework.Exceptions.Common;
using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prost.Api.Common.Caching;
using Prost.Api.Common.Options;
using Prost.Api.Common.Services;
using Prost.Api.Data;
using Prost.Api.Data.Exceptions;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.ValueObjects;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Accounts;

internal sealed class UpdateEmailHandler(
    ProstDbContext context,
    TimeProvider time,
    IFusionCache cache,
    ITokenHasher tokenHasher,
    IAccessTokenIssuer accessTokenIssuer,
    IRefreshTokenIssuer refreshTokenIssuer,
    IRevokedTokenFamilyCache revokedTokenFamilyCache,
    IOptions<RefreshTokenOptions> options
) : IRequestHandler<UpdateEmailCommand, ErrorOr<UpdateEmailResponse>>
{
    public async ValueTask<ErrorOr<UpdateEmailResponse>> Handle(
        UpdateEmailCommand command,
        CancellationToken cancellationToken)
    {
        var email = Email.Create(command.Email);

        if (email.IsError) return email.Errors;

        if (await context.Accounts.SingleOrDefaultAsync(
            x => x.Id == command.AccountId,
            cancellationToken) is not { } account)
        {
            return UpdateEmailErrors.NotFound;
        }
        account.UpdateEmail(email.Value);

        var oldTokenFamilies = await context.TokenFamilies
            .Where(x =>
                x.AccountId == command.AccountId &&
                x.RevokedAt == null)
            .ToListAsync(cancellationToken);

        var now = time.GetUtcNow();

        foreach (var oldTokenFamily in oldTokenFamilies)
        {
            oldTokenFamily.Revoke(now);
        }
        var issuedRefreshToken = refreshTokenIssuer.Issue();

        var tokenHash = TokenHash.Create(
            tokenHasher.Hash(issuedRefreshToken));

        if (tokenHash.IsError) return tokenHash.Errors;

        var tokenFamily = TokenFamily.Create(
            TokenFamilyId.Create(),
            account.Id);

        var expiresAt = now.AddDays(
            options.Value.ExpirationInDays);

        var refreshToken = RefreshToken.Create(
            RefreshTokenId.Create(),
            tokenFamily.Id,
            tokenHash.Value,
            expiresAt: expiresAt,
            now: now);

        if (refreshToken.IsError) return refreshToken.Errors;

        context.TokenFamilies.Add(tokenFamily);

        context.RefreshTokens.Add(refreshToken.Value);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintException ex) when (ex.RefersTo<Email>())
        {
            return UpdateEmailErrors.Taken;
        }
        catch (DbUpdateConcurrencyException)
        {
            return UpdateEmailErrors.ConcurrencyConflict;
        }
        await Task.WhenAll(
            oldTokenFamilies.Select(x =>
                revokedTokenFamilyCache.RevokeAsync(
                    x.Id,
                    cancellationToken)));

        await cache.RemoveByTagAsync(
            $"accounts:{command.AccountId}",
            token: cancellationToken);

        var issuedAccessToken = accessTokenIssuer.Issue(
            tokenFamily.AccountId,
            tokenFamily.Id);

        return new UpdateEmailResponse(
            AccessToken: issuedAccessToken,
            RefreshToken: issuedRefreshToken);
    }
}
