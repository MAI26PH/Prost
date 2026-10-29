using ErrorOr;
using Prost.Api.Domain.Primitives;
using System.Text;
using System.Text.RegularExpressions;

namespace Prost.Api.Domain.ValueObjects;

internal sealed partial record Username : ValueObject<string>
{
    public const int MinLength = 2;
    public const int MaxLength = 64;

    private const string Letters = "a-zA-ZñÑáÁéÉíÍóÓúÚ";
    private const string Digits = "0-9";
    private const string Separators = @"\-_.";

    [GeneratedRegex(
        $@"^[{Letters}{Digits}{Separators}]+\z",
        RegexOptions.CultureInvariant)]
    private static partial Regex SupportedCharactersRegex();

    [GeneratedRegex(
        $@"[{Separators}]{{2,}}",
        RegexOptions.CultureInvariant)]
    private static partial Regex ConsecutiveSeparatorsRegex();

    [GeneratedRegex(
        $@"^[{Letters}]",
        RegexOptions.CultureInvariant)]
    private static partial Regex StartsWithLetterRegex();

    [GeneratedRegex(
        $@"[{Letters}{Digits}]\z",
        RegexOptions.CultureInvariant)]
    private static partial Regex EndsWithLetterOrDigitRegex();

    private Username(string value) : base(value) { }

    public static ErrorOr<Username> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return UsernameErrors.Empty;
        }
        value = value.Trim();

        try
        {
            value = value.Normalize(NormalizationForm.FormC);
        }
        catch (ArgumentException)
        {
            return UsernameErrors.UnsupportedCharacters;
        }
        var errors = new List<Error>();

        if (value.Length < MinLength || value.Length > MaxLength)
        {
            errors.Add(UsernameErrors.InvalidLength);
        }
        if (!SupportedCharactersRegex().IsMatch(value))
        {
            errors.Add(UsernameErrors.UnsupportedCharacters);
        }
        if (ConsecutiveSeparatorsRegex().IsMatch(value))
        {
            errors.Add(UsernameErrors.ConsecutiveSeparators);
        }
        if (!StartsWithLetterRegex().IsMatch(value))
        {
            errors.Add(UsernameErrors.DoesNotStartWithLetter);
        }
        if (!EndsWithLetterOrDigitRegex().IsMatch(value))
        {
            errors.Add(UsernameErrors.DoesNotEndWithLetterOrDigit);
        }
        if (errors.Count > 0) return errors;

        return new Username(value.ToLowerInvariant());
    }
}
