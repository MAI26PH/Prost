using ErrorOr;

namespace Prost.Api.Domain.ValueObjects;

internal static class TitleErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "Title.Empty",
        description: "Title cannot be empty.");

    public static readonly Error ExceedsMaxLength = Error.Validation(
        code: "Title.ExceedsMaxLength",
        description: $"Title must not exceed {Title.MaxLength} characters.");
}
