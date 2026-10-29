using Microsoft.IdentityModel.JsonWebTokens;
using Prost.Api.Domain.Entities;
using System.Security.Claims;

namespace Prost.Api.Common.Claims;

internal static class ClaimsPrincipalExtensions
{
    extension(ClaimsPrincipal principal)
    {
        public AccountId AccountId
        {
            get
            {
                var sub = principal.FindFirstValue(
                    JwtRegisteredClaimNames.Sub);

                if (!Guid.TryParse(sub, out var value))
                {
                    return default;
                }
                return AccountId.From(value);
            }
        }

        public TokenFamilyId TokenFamilyId
        {
            get
            {
                var sid = principal.FindFirstValue(
                    JwtRegisteredClaimNames.Sid);

                if (!Guid.TryParse(sid, out var value))
                {
                    return default;
                }
                return TokenFamilyId.From(value);
            }
        }
    }
}
