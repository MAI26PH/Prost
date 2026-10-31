using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prost.Api.Common.Caching;
using Prost.Api.Common.Options;
using Prost.Api.Common.Services;
using Prost.Api.Data;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.ValueObjects;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Accounts;

internal sealed class UpdatePasswordHandler(
    ProstDbContext context,
    TimeProvider time,
    IFusionCache cache,
    ITokenHasher tokenHasher,
    IPasswordHasher passwordHasher,
    IAccessTokenIssuer accessTokenIssuer,
    IRefreshTokenIssuer refreshTokenIssuer,
    IRevokedTokenFamilyCache revokedTokenFamilyCache,
    IOptions<RefreshTokenOptions> options
) : IRequestHandler<UpdatePasswordCommand, ErrorOr<UpdatePasswordResponse>>
{
    public async ValueTask<ErrorOr<UpdatePasswordResponse>> Handle(
        UpdatePasswordCommand command,
        CancellationToken cancellationToken)
    {
        var password = Password.Create(command.Password);

        if (password.IsError) return password.Errors;

        var account = await context.Accounts
            .Include(x => x.PasswordCredential)
            .SingleOrDefaultAsync(
                x => x.Id == command.AccountId,
                cancellationToken);

        if (account is null) return UpdatePasswordErrors.NotFound;

        if (account.PasswordCredential is not { } passwordCredential)
        {
            return UpdatePasswordErrors.NotSet;
        }
        var passwordHash = PasswordHash.Create(
            passwordHasher.Hash(password.Value.Value));

        if (passwordHash.IsError) return passwordHash.Errors;

        passwordCredential.UpdatePasswordHash(passwordHash.Value);

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
        catch (DbUpdateConcurrencyException)
        {
            return UpdatePasswordErrors.ConcurrencyConflict;
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

        return new UpdatePasswordResponse(
            AccessToken: issuedAccessToken,
            RefreshToken: issuedRefreshToken);
    }
}
