using Prost.Api.Common.Attributes;
using System.Reflection;

namespace Prost.Api.Common.DependencyInjection;

internal static class OptionsDependencyInjectionExtensions
{
    private static IServiceCollection AddOptionsFromAttribute<T>(
        this IServiceCollection services,
        OptionsAttribute attribute
    ) where T : class
    {
        services
            .AddOptions<T>()
            .BindConfiguration(attribute.Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddOptionsFromAttributedType<T>(
        this IServiceCollection services
    ) where T : class
    {
        var attribute = typeof(T).GetCustomAttribute<OptionsAttribute>()
            ?? throw new InvalidOperationException(
                $"{typeof(T).FullName} is not attributed " +
                $"with {nameof(OptionsAttribute)}.");

        services.AddOptionsFromAttribute<T>(attribute);

        return services;
    }

    public static IServiceCollection AddOptionsFromAssembly(
        this IServiceCollection services,
        Assembly assembly)
    {
        var method = typeof(OptionsDependencyInjectionExtensions).GetMethod(
            nameof(AddOptionsFromAttribute),
            BindingFlags.NonPublic | BindingFlags.Static);

        if (method is null) return services;

        foreach (var type in assembly.GetTypes())
        {
            if (type is { IsClass: false } or { IsAbstract: true }
                or { ContainsGenericParameters: true }) continue;

            var attribute = type.GetCustomAttribute<OptionsAttribute>();

            if (attribute is null) continue;

            method.MakeGenericMethod(type).Invoke(null, [services, attribute]);
        }
        return services;
    }
}
