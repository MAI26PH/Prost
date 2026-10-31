using Microsoft.AspNetCore.Authentication.JwtBearer;
using Prost.Api.Common.Caching;
using Prost.Api.Common.Claims;

namespace Prost.Api.Features.Authentication.JwtBearer;

internal static class JwtBearerEventsHandler
{
    public static async Task OnTokenValidated(
        this TokenValidatedContext context)
    {
        if (context.Principal is null)
        {
            context.Fail("The token principal is null.");

            return;
        }
        var revokedTokenFamilyCache = context.HttpContext.RequestServices
            .GetRequiredService<IRevokedTokenFamilyCache>();

        if (await revokedTokenFamilyCache.IsRevokedAsync(
            context.Principal.TokenFamilyId,
            context.HttpContext.RequestAborted))
        {
            context.Fail("The token family has been revoked.");
        }
    }
}
