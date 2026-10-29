using Microsoft.Extensions.DependencyInjection.Extensions;
using Prost.Api.Common.Routing;
using System.Reflection;

namespace Prost.Api.Common.DependencyInjection;

internal static class EndpointDependencyInjectionExtensions
{
    public static IServiceCollection AddEndpointsFromAssembly(
        this IServiceCollection services,
        Assembly assembly)
    {
        var descriptors = assembly.DefinedTypes
            .Where(type =>
                type is { IsAbstract: false, IsInterface: false } &&
                type.IsAssignableTo(typeof(IEndpoint)))
            .SelectMany(info =>
                info.GetInterfaces()
                    .Where(type =>
                        type.IsGenericType &&
                        type.GetGenericTypeDefinition() == typeof(IEndpoint<>))
                    .Select(type =>
                        ServiceDescriptor.Transient(type, info)))
            .ToArray();

        services.TryAddEnumerable(descriptors);

        return services;
    }

    public static void MapEndpointsFromConfiguration<TEndpointConfiguration>(
        this IApplicationBuilder app,
        IEndpointRouteBuilder builder
    ) where TEndpointConfiguration : EndpointConfiguration
    {
        var endpoints = app.ApplicationServices
            .GetRequiredService<IEnumerable<IEndpoint<TEndpointConfiguration>>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }
    }
}
