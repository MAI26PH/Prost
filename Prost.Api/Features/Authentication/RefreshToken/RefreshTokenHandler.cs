using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prost.Api.Common.Services;
using Prost.Api.Data;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.ValueObjects;
using Prost.Api.Features.Authentication.Options;

namespace Prost.Api.Features.Authentication;

internal sealed class RefreshTokenHandler(
    ProstDbContext context,
    TimeProvider time,
    ITokenHasher tokenHasher,
    IAccessTokenIssuer accessTokenIssuer,
    IRefreshTokenIssuer refreshTokenIssuer,
    IRevokedTokenFamilyCache revokedTokenFamilyCache,
    IOptions<RefreshTokenOptions> options
) : IRequestHandler<RefreshTokenCommand, ErrorOr<RefreshTokenResponse>>
{
    public async ValueTask<ErrorOr<RefreshTokenResponse>> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var tokenHash = TokenHash.Create(
            tokenHasher.Hash(command.RefreshToken));

        if (tokenHash.IsError) return tokenHash.Errors;

        var refreshToken = await context.RefreshTokens
            .Include(x => x.TokenFamily)
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash.Value,
                cancellationToken);

        if (refreshToken is not { TokenFamily: { } tokenFamily })
        {
            return RefreshTokenErrors.Invalid;
        }
        var issuedRefreshToken = refreshTokenIssuer.Issue();

        var newTokenHash = TokenHash.Create(
            tokenHasher.Hash(issuedRefreshToken));

        if (newTokenHash.IsError) return newTokenHash.Errors;

        var now = time.GetUtcNow();

        var expiresAt = now.AddDays(
            options.Value.ExpirationInDays);

        var newRefreshToken = refreshToken.Rotate(
            RefreshTokenId.Create(),
            tokenFamily,
            newTokenHash.Value,
            expiresAt: expiresAt,
            now: now);

        if (newRefreshToken.IsError)
        {
            if (newRefreshToken.HasConsumedError)
            {
                tokenFamily.Revoke(now);

                await context.SaveChangesAsync(cancellationToken);

                await revokedTokenFamilyCache.RevokeAsync(
                    tokenFamily.Id,
                    cancellationToken);
            }
            return newRefreshToken.Errors;
        }
        context.RefreshTokens.Add(newRefreshToken.Value);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return RefreshTokenErrors.ConcurrencyConflict;
        }
        var issuedAccessToken = accessTokenIssuer.Issue(
            tokenFamily.AccountId,
            tokenFamily.Id);

        return new RefreshTokenResponse(
            AccessToken: issuedAccessToken,
            RefreshToken: issuedRefreshToken);
    }
}
