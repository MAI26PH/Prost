namespace Prost.Api.Common.Attributes;

[AttributeUsage(
    AttributeTargets.Class,
    Inherited = false)]
public class ServiceAttribute(
    ServiceLifetime lifetime,
    Type? type = null
) : Attribute
{
    public ServiceLifetime Lifetime { get; } = lifetime;
    public Type? Type { get; } = type;
}

[AttributeUsage(
    AttributeTargets.Class,
    Inherited = false)]
public sealed class ServiceAttribute<TService>(
    ServiceLifetime lifetime
) : ServiceAttribute(lifetime, typeof(TService));
