using ErrorOr;

namespace Prost.Api.Domain.ValueObjects;

internal static class PasswordErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "Password.Empty",
        description: "Password cannot be empty.");

    public static readonly Error InvalidLength = Error.Validation(
        code: "Password.InvalidLength",
        description: $"Password must be between {Password.MinLength} and {Password.MaxLength} characters.");

    public static readonly Error WhiteSpace = Error.Validation(
        code: "Password.WhiteSpace",
        description: "Password must not contain white space.");
}
