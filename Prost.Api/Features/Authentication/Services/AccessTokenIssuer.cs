using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Prost.Api.Common.Attributes;
using Prost.Api.Domain.Entities;
using Prost.Api.Features.Authentication.Options;
using System.Security.Claims;
using System.Text;

namespace Prost.Api.Features.Authentication.Services;

internal interface IAccessTokenIssuer
{
    Task<string> IssueAsync(
        AccountId accountId,
        TokenFamilyId tokenFamilyId,
        Func<TokenFamilyId, DateTimeOffset, CancellationToken, Task> onIssued,
        CancellationToken cancellationToken = default);
}

[Service<IAccessTokenIssuer>(ServiceLifetime.Singleton)]
internal sealed class AccessTokenIssuer(
    TimeProvider time,
    IOptions<JwtOptions> options
) : IAccessTokenIssuer
{
    private readonly SigningCredentials signingCredentials = new(
        new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(options.Value.SigningKey)),
        options.Value.Algorithm);

    private readonly JsonWebTokenHandler handler = new();

    public async Task<string> IssueAsync(
        AccountId accountId,
        TokenFamilyId tokenFamilyId,
        Func<TokenFamilyId, DateTimeOffset, CancellationToken, Task> onIssued,
        CancellationToken cancellationToken = default)
    {
        var expiresAt = time.GetUtcNow().AddMinutes(
            options.Value.ExpirationInMinutes);

        List<Claim> claims = [
            new Claim(
                JwtRegisteredClaimNames.Sub,
                accountId.ToString()),
            new Claim(
                JwtRegisteredClaimNames.Sid,
                tokenFamilyId.ToString()),
        ];
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = options.Value.Issuer,
            Audience = options.Value.Audience,
            Expires = expiresAt.UtcDateTime,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingCredentials
        };
        var token = handler.CreateToken(descriptor);

        await onIssued(
            tokenFamilyId,
            expiresAt,
            cancellationToken);

        return token;
    }
}
