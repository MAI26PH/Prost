using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Prost.Api.Data;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.ValueObjects;
using Prost.Api.Features.Authentication.Options;
using Prost.Api.Features.Authentication.Services;

namespace Prost.Api.Features.Authentication;

internal sealed class SignInHandler(
    ProstDbContext context,
    TimeProvider time,
    ITokenHasher tokenHasher,
    IPasswordHasher passwordHasher,
    IAccessTokenIssuer accessTokenIssuer,
    IRefreshTokenIssuer refreshTokenIssuer,
    ITokenFamilyRevocation tokenFamilyRevocation,
    IOptions<RefreshTokenOptions> options
) : IRequestHandler<SignInCommand, ErrorOr<SignInResponse>>
{
    public async ValueTask<ErrorOr<SignInResponse>> Handle(
        SignInCommand command,
        CancellationToken cancellationToken)
    {
        var email = Email.Create(command.Email);

        if (email.IsError) return email.Errors;

        var account = await context.Accounts
            .Include(x => x.PasswordCredential)
            .FirstOrDefaultAsync(
                x => x.Email == email.Value,
                cancellationToken);

        var verified = passwordHasher.Verify(
            command.Password,
            account?.PasswordCredential?.PasswordHash?.Value);

        if (!verified || account is null)
        {
            return SignInErrors.InvalidCredentials;
        }
        var issuedRefreshToken = refreshTokenIssuer.Issue();

        var tokenHash = TokenHash.Create(
            tokenHasher.Hash(issuedRefreshToken));

        if (tokenHash.IsError) return tokenHash.Errors;

        var tokenFamily = TokenFamily.Create(
            TokenFamilyId.Create(),
            account.Id);

        var now = time.GetUtcNow();

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

        await context.SaveChangesAsync(cancellationToken);

        var issuedAccessToken = await accessTokenIssuer.IssueAsync(
            tokenFamily.AccountId,
            tokenFamily.Id,
            tokenFamilyRevocation.SetAsync,
            cancellationToken);

        return new SignInResponse(
            AccessToken: issuedAccessToken,
            RefreshToken: issuedRefreshToken);
    }
}
