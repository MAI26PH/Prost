namespace Prost.Api.Common.Attributes;

[AttributeUsage(
    AttributeTargets.Class,
    Inherited = false)]
internal class ServiceAttribute(
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
internal sealed class ServiceAttribute<TService>(
    ServiceLifetime lifetime
) : ServiceAttribute(lifetime, typeof(TService));
