using ErrorOr;

namespace Prost.Api.Features.Posts;

internal static class UpdatePostErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: "UpdatePost.NotFound",
        description: "The post was not found.");

    public static readonly Error ConcurrencyConflict = Error.Conflict(
        code: $"UpdatePost.ConcurrencyConflict",
        description: "The update failed due to a concurrency conflict.");
}
