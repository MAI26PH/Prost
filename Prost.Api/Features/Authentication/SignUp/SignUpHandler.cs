using EntityFramework.Exceptions.Common;
using ErrorOr;
using Mediator;
using Microsoft.Extensions.Options;
using Prost.Api.Common.Services;
using Prost.Api.Data;
using Prost.Api.Data.Exceptions;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.ValueObjects;
using Prost.Api.Features.Authentication.Options;

namespace Prost.Api.Features.Authentication;

internal sealed class SignUpHandler(
    ProstDbContext context,
    TimeProvider time,
    ITokenHasher tokenHasher,
    IPasswordHasher passwordHasher,
    IAccessTokenIssuer accessTokenIssuer,
    IRefreshTokenIssuer refreshTokenIssuer,
    IOptions<RefreshTokenOptions> options
) : IRequestHandler<SignUpCommand, ErrorOr<SignUpResponse>>
{
    public async ValueTask<ErrorOr<SignUpResponse>> Handle(
        SignUpCommand command,
        CancellationToken cancellationToken)
    {
        var username = Username.Create(command.Username);

        var email = Email.Create(command.Email);

        var password = Password.Create(command.Password);

        var errors = new List<Error>();

        if (username.IsError) errors.AddRange(username.Errors);

        if (email.IsError) errors.AddRange(email.Errors);

        if (password.IsError) errors.AddRange(password.Errors);

        if (errors.Count > 0) return errors;

        var account = Account.Create(
            AccountId.Create(),
            username.Value,
            email.Value);

        var passwordHash = PasswordHash.Create(
            passwordHasher.Hash(password.Value.Value));

        if (passwordHash.IsError) return passwordHash.Errors;

        var passwordCredential = PasswordCredential.Create(
            PasswordCredentialId.Create(),
            account.Id,
            passwordHash.Value);

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

        context.Accounts.Add(account);

        context.PasswordCredentials.Add(passwordCredential);

        context.TokenFamilies.Add(tokenFamily);

        context.RefreshTokens.Add(refreshToken.Value);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintException ex) when (ex.RefersTo<Username>())
        {
            return SignUpErrors.UsernameTaken;
        }
        catch (UniqueConstraintException ex) when (ex.RefersTo<Email>())
        {
            return SignUpErrors.EmailTaken;
        }
        var issuedAccessToken = accessTokenIssuer.Issue(
            tokenFamily.AccountId,
            tokenFamily.Id);

        return new SignUpResponse(
            AccessToken: issuedAccessToken,
            RefreshToken: issuedRefreshToken);
    }
}
