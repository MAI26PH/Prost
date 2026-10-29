using ErrorOr;

namespace Prost.Api.Domain.ValueObjects;

internal static class TokenHashErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "TokenHash.Empty",
        description: "Token hash cannot be empty.");

    public static readonly Error ExceedsMaxLength = Error.Validation(
        code: "TokenHash.ExceedsMaxLength",
        description: $"Token hash must not exceed {TokenHash.MaxLength} characters.");

    public static readonly Error UnsupportedFormat = Error.Validation(
        code: "TokenHash.UnsupportedFormat",
        description: "Token hash uses an unsupported format.");
}
