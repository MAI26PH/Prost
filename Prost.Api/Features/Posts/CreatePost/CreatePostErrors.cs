using ErrorOr;

namespace Prost.Api.Features.Posts;

internal static class CreatePostErrors
{
    public static readonly Error SlugTaken = Error.Conflict(
        code: "CreatePost.SlugTaken",
        description: "The slug is already taken.");
}
