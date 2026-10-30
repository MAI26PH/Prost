using ErrorOr;

namespace Prost.Api.Features.Posts;

internal static class GetPostErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: "GetPost.NotFound",
        description: "The post was not found.");
}
