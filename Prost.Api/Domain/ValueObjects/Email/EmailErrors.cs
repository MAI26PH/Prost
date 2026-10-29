using ErrorOr;

namespace Prost.Api.Domain.ValueObjects;

internal static class EmailErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "Email.Empty",
        description: "Email address cannot be empty.");

    public static readonly Error ExceedsMaxLength = Error.Validation(
        code: "Email.ExceedsMaxLength",
        description: $"Email address must not exceed {Email.MaxLength} characters.");

    public static readonly Error UnsupportedFormat = Error.Validation(
        code: "Email.UnsupportedFormat",
        description: "Email address uses an unsupported format.");
}
