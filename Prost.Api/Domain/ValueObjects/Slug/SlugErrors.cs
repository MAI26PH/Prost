using ErrorOr;

namespace Prost.Api.Domain.ValueObjects;

internal static class SlugErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "Slug.Empty",
        description: "Slug cannot be empty.");

    public static readonly Error InvalidLength = Error.Validation(
        code: "Slug.InvalidLength",
        description: $"Slug must be between {Slug.MinLength} and {Slug.MaxLength} characters.");

    public static readonly Error UnsupportedCharacters = Error.Validation(
        code: "Slug.UnsupportedCharacters",
        description: "Slug must be in [ a-z, A-Z ] letters, [ 0-9 ] digits, and [ - ] separator.");

    public static readonly Error ConsecutiveSeparator = Error.Validation(
        code: "Slug.ConsecutiveSeparator",
        description: "Slug must not have consecutive [ - ] separator.");

    public static readonly Error DoesNotStartWithLetterOrDigit = Error.Validation(
        code: "Slug.DoesNotStartWithLetterOrDigit",
        description: "Slug must start with a letter or digit.");

    public static readonly Error DoesNotEndWithLetterOrDigit = Error.Validation(
        code: "Slug.DoesNotEndWithLetterOrDigit",
        description: "Slug must end with a letter or digit.");
}
