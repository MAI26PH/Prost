using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Prost.Api.Features.Authentication.JwtBearer;
using Prost.Api.Features.Authentication.Options;
using System.Text;

namespace Prost.Api.Features.Authentication;

internal static class AuthenticationDependencyInjectionExtensions
{
    public static void AddAuthenticationFeature(
        this IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services
            .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((bearerOptions, jwtOptions) =>
            {
                bearerOptions.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Value.Issuer,
                    ValidAudience = jwtOptions.Value.Audience,
                    ValidAlgorithms = [jwtOptions.Value.Algorithm],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Value.SigningKey)),
                };
                bearerOptions.Events = new()
                {
                    OnTokenValidated = JwtBearerEventsHandler.OnTokenValidated
                };
                bearerOptions.MapInboundClaims = false;
            });
    }
}
