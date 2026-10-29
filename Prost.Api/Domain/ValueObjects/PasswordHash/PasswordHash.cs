using ErrorOr;
using Prost.Api.Domain.Primitives;
using System.Text.RegularExpressions;

namespace Prost.Api.Domain.ValueObjects;

internal sealed partial record PasswordHash : ValueObject<string>
{
    public const int MaxLength = 60;

    [GeneratedRegex(
        @"^\$2[aby]\$(?:0[4-9]|[12][0-9]|3[01])\$[./A-Za-z0-9]{53}\z",
        RegexOptions.CultureInvariant)]
    private static partial Regex BCryptFormatRegex();

    private PasswordHash(string value) : base(value) { }

    public static ErrorOr<PasswordHash> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return PasswordHashErrors.Empty;
        }
        if (value.Length > MaxLength)
        {
            return PasswordHashErrors.ExceedsMaxLength;
        }
        if (!BCryptFormatRegex().IsMatch(value))
        {
            return PasswordHashErrors.UnsupportedFormat;
        }
        return new PasswordHash(value);
    }
}
