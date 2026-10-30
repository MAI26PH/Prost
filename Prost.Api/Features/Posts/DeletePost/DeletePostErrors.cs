using ErrorOr;

namespace Prost.Api.Features.Posts;

internal static class DeletePostErrors
{
    public static readonly Error Published = Error.Conflict(
        code: "DeletePost.Published",
        description: "Cannot delete a published post, unpublish it first.");
}
