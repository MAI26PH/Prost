using ErrorOr;
using Prost.Api.Domain.Primitives;

namespace Prost.Api.Domain.ValueObjects;

internal sealed record Title : ValueObject<string>
{
    public const int MaxLength = 256;

    private Title(string value) : base(value) { }

    public static ErrorOr<Title> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return TitleErrors.Empty;
        }
        value = value.Trim();

        if (value.Length > MaxLength)
        {
            return TitleErrors.ExceedsMaxLength;
        }
        return new Title(value);
    }
}
