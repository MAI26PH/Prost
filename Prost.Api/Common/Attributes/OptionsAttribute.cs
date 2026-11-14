namespace Prost.Api.Common.Attributes;

[AttributeUsage(
    AttributeTargets.Class,
    Inherited = true)]
public sealed class OptionsAttribute(
    string name
) : Attribute
{
    public string Name { get; } =
        !string.IsNullOrWhiteSpace(name)
            ? name
            : throw new ArgumentException(
                "Name cannot be empty.",
                nameof(name));
}
