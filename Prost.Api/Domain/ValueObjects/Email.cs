using Prost.Api.Domain.Primitives;
using System.Text.RegularExpressions;

namespace Prost.Api.Domain.ValueObjects;

public sealed partial record Email : ValueObject<string>
{
    public static class Errors
    {
        public static readonly Error Empty = Error.Validation(
            code: "Email.Empty",
            description: "Email address cannot be empty.");

        public static readonly Error ExceedsMaxLength = Error.Validation(
            code: "Email.ExceedsMaxLength",
            description: $"Email address must not exceed {MaxLength} characters.");

        public static readonly Error UnsupportedFormat = Error.Validation(
            code: "Email.UnsupportedFormat",
            description: "Email address uses an unsupported format.");
    }

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
            return Errors.Empty;
        }
        value = value.Trim();

        if (value.Length > MaxLength)
        {
            return Errors.ExceedsMaxLength;
        }
        if (!SupportedFormatRegex().IsMatch(value))
        {
            return Errors.UnsupportedFormat;
        }
        return new Email(value.ToLowerInvariant());
    }
}
