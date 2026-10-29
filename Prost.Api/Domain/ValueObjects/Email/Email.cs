using ErrorOr;
using Prost.Api.Domain.Primitives;
using System.Text.RegularExpressions;

namespace Prost.Api.Domain.ValueObjects;

internal sealed partial record Email : ValueObject<string>
{
    public const int MaxLength = 254;

    [GeneratedRegex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.CultureInvariant)]
    private static partial Regex SupportedFormatRegex();

    private Email(string value) : base(value) { }

    public static ErrorOr<Email> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return EmailErrors.Empty;
        }
        value = value.Trim();

        if (value.Length > MaxLength)
        {
            return EmailErrors.ExceedsMaxLength;
        }
        if (!SupportedFormatRegex().IsMatch(value))
        {
            return EmailErrors.UnsupportedFormat;
        }
        return new Email(value.ToLowerInvariant());
    }
}
