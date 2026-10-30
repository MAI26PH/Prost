using ErrorOr;

namespace Prost.Api.Domain.ValueObjects;

internal static class BodyErrors
{
    public static readonly Error Empty = Error.Validation(
        code: "Body.Empty",
        description: "Body cannot be empty.");

    public static readonly Error Unsupported = Error.Validation(
        code: "Body.Unsupported",
        description: "Body must be a JSON object.");
}
