using Prost.Api.Common.Attributes;
using System.Reflection;

namespace Prost.Api.Common.DependencyInjection;

internal static class ServiceDependencyInjectionExtensions
{
    public static IServiceCollection AddServicesFromAssembly(
        this IServiceCollection services,
        Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type is { IsClass: false } or { IsAbstract: true }
                or { ContainsGenericParameters: true }) continue;

            var attribute = type.GetCustomAttribute<ServiceAttribute>();

            if (attribute is null) continue;

            if (attribute.Type is not null && !attribute.Type.IsAssignableFrom(type))
            {
                throw new InvalidOperationException(
                    $"{type.FullName} is registered as " +
                    $"{attribute.Type.FullName} but does not implement it.");
            }
            services.Add(new ServiceDescriptor(
                attribute.Type ?? type,
                type,
                attribute.Lifetime));
        }
        return services;
    }
}
