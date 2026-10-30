using ErrorOr;
using Prost.Api.Domain.Primitives;
using System.Text.RegularExpressions;

namespace Prost.Api.Domain.ValueObjects;

internal sealed partial record Slug : ValueObject<string>
{
    public const int MinLength = 2;
    public const int MaxLength = 128;

    private const string Letters = "a-zA-Z";
    private const string Digits = "0-9";
    private const string Separator = @"\-";

    [GeneratedRegex(
        $@"^[{Letters}{Digits}{Separator}]+\z",
        RegexOptions.CultureInvariant)]
    private static partial Regex SupportedCharactersRegex();

    [GeneratedRegex(
        $@"[{Separator}]{{2,}}",
        RegexOptions.CultureInvariant)]
    private static partial Regex ConsecutiveSeparatorRegex();

    [GeneratedRegex(
        $@"^[{Letters}{Digits}]",
        RegexOptions.CultureInvariant)]
    private static partial Regex StartsWithLetterOrDigitRegex();

    [GeneratedRegex(
        $@"[{Letters}{Digits}]\z",
        RegexOptions.CultureInvariant)]
    private static partial Regex EndsWithLetterOrDigitRegex();

    private Slug(string value) : base(value) { }

    public static ErrorOr<Slug> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return SlugErrors.Empty;
        }
        value = value.Trim();

        var errors = new List<Error>();

        if (value.Length < MinLength || value.Length > MaxLength)
        {
            errors.Add(SlugErrors.InvalidLength);
        }
        if (!SupportedCharactersRegex().IsMatch(value))
        {
            errors.Add(SlugErrors.UnsupportedCharacters);
        }
        if (ConsecutiveSeparatorRegex().IsMatch(value))
        {
            errors.Add(SlugErrors.ConsecutiveSeparator);
        }
        if (!StartsWithLetterOrDigitRegex().IsMatch(value))
        {
            errors.Add(SlugErrors.DoesNotStartWithLetterOrDigit);
        }
        if (!EndsWithLetterOrDigitRegex().IsMatch(value))
        {
            errors.Add(SlugErrors.DoesNotEndWithLetterOrDigit);
        }
        if (errors.Count > 0) return errors;

        return new Slug(value.ToLowerInvariant());
    }
}
