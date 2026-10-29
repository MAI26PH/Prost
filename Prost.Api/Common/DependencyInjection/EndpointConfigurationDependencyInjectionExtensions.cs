using Microsoft.Extensions.DependencyInjection.Extensions;
using Prost.Api.Common.Routing;
using System.Reflection;

namespace Prost.Api.Common.DependencyInjection;

internal static class EndpointConfigurationDependencyInjectionExtensions
{
    public static IServiceCollection AddEndpointConfigurationsFromAssembly(
        this IServiceCollection services,
        Assembly assembly)
    {
        var descriptors = assembly.DefinedTypes
            .Where(type =>
                type is { IsAbstract: false, IsInterface: false } &&
                type.IsAssignableTo(typeof(EndpointConfiguration)))
            .Select(
                type => ServiceDescriptor.Transient(
                    typeof(EndpointConfiguration),
                    type))
            .ToArray();

        services.TryAddEnumerable(descriptors);

        return services;
    }

    public static void MapEndpointConfigurations(
        this IApplicationBuilder app,
        IEndpointRouteBuilder builder)
    {
        var configurations = app.ApplicationServices
            .GetRequiredService<IEnumerable<EndpointConfiguration>>();

        foreach (var configuration in configurations)
        {
            configuration.MapEndpointConfiguration(app, builder);
        }
    }
}
