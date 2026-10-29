namespace Prost.Api.Common.Attributes;

[AttributeUsage(
    AttributeTargets.Class,
    Inherited = true)]
internal sealed class OptionsAttribute(
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
