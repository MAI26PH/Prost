using ErrorOr;
using Prost.Api.Domain.Primitives;
using System.Text.RegularExpressions;

namespace Prost.Api.Domain.ValueObjects;

internal sealed partial record TokenHash : ValueObject<string>
{
    public const int MaxLength = 64;

    [GeneratedRegex(
        @"^[A-Fa-f0-9]{64}\z",
        RegexOptions.CultureInvariant)]
    private static partial Regex Sha256FormatRegex();

    private TokenHash(string value) : base(value) { }

    public static ErrorOr<TokenHash> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return TokenHashErrors.Empty;
        }
        if (value.Length > MaxLength)
        {
            return TokenHashErrors.ExceedsMaxLength;
        }
        if (!Sha256FormatRegex().IsMatch(value))
        {
            return TokenHashErrors.UnsupportedFormat;
        }
        return new TokenHash(value);
    }
}
