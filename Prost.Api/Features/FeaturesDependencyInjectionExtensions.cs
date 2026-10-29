using Prost.Api.Features.Authentication;

namespace Prost.Api.Features;

internal static class FeaturesDependencyInjectionExtensions
{
    public static IServiceCollection AddFeatures(
        this IServiceCollection services)
    {
        services.AddAuthenticationFeature();

        return services;
    }

    public static IApplicationBuilder UseFeatures(
        this IApplicationBuilder app)
    {
        app.UseAuthentication();

        return app;
    }
}
