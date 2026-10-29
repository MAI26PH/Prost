namespace Prost.Api.Features;

internal static class FeaturesDependencyInjectionExtensions
{
    public static IServiceCollection AddFeatures(
        this IServiceCollection services) =>
        services;

    public static IApplicationBuilder UseFeatures(
        this IApplicationBuilder app) =>
        app;
}
