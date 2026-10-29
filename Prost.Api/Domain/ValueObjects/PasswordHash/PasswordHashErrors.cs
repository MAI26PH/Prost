using ErrorOr;

namespace Prost.Api.Domain.ValueObjects;

internal static class PasswordHashErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "PasswordHash.Empty",
        description: "Password hash cannot be empty.");

    public static readonly Error ExceedsMaxLength = Error.Validation(
        code: "PasswordHash.ExceedsMaxLength",
        description: $"Password hash must not exceed {PasswordHash.MaxLength} characters.");

    public static readonly Error UnsupportedFormat = Error.Validation(
        code: "PasswordHash.UnsupportedFormat",
        description: "Password hash uses an unsupported format.");
}
