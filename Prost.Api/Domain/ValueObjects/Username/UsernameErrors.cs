using ErrorOr;

namespace Prost.Api.Domain.ValueObjects;

internal static class UsernameErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "Username.Empty",
        description: "Username cannot be empty.");

    public static readonly Error InvalidLength = Error.Validation(
        code: "Username.InvalidLength",
        description: $"Username must be between {Username.MinLength} and {Username.MaxLength} characters.");

    public static readonly Error UnsupportedCharacters = Error.Validation(
        code: "Username.UnsupportedCharacters",
        description: "Username must be in [ a-z, A-Z ] letters including ñ and acute vowels, [ 0-9 ] digits, and [ -, _, . ] separators.");

    public static readonly Error ConsecutiveSeparators = Error.Validation(
        code: "Username.ConsecutiveSeparators",
        description: "Username must not have consecutive [ -, _, . ] separators.");

    public static readonly Error DoesNotStartWithLetter = Error.Validation(
        code: "Username.DoesNotStartWithLetter",
        description: "Username must start with a letter.");

    public static readonly Error DoesNotEndWithLetterOrDigit = Error.Validation(
        code: "Username.DoesNotEndWithLetterOrDigit",
        description: "Username must end with a letter or digit.");
}
