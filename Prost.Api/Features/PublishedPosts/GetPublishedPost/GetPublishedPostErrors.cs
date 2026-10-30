using ErrorOr;

namespace Prost.Api.Features.PublishedPosts;

internal static class GetPublishedPostErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: "GetPublishedPost.NotFound",
        description: "The published post was not found.");
}
