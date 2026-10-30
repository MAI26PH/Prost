using ErrorOr;

namespace Prost.Api.Features.Posts;

internal static class RepublishPostErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: "RepublishPost.NotFound",
        description: "The post was not found.");

    public static readonly Error NotPublished = Error.NotFound(
        code: "RepublishPost.NotPublished",
        description: "The post is not published.");

    public static readonly Error ConcurrencyConflict = Error.Conflict(
        code: $"RepublishPost.ConcurrencyConflict",
        description: "The republish failed due to a concurrency conflict.");
}
