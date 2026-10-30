using ErrorOr;

namespace Prost.Api.Features.Posts;

internal static class PublishPostErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: "PublishPost.NotFound",
        description: "The post was not found.");

    public static readonly Error Published = Error.Conflict(
        code: "PublishPost.Published",
        description: "The post is already published.");
}
