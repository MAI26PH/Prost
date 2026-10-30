using EntityFramework.Exceptions.Common;
using ErrorOr;
using Mediator;
using Prost.Api.Data;
using Prost.Api.Data.Exceptions;
using Prost.Api.Domain.Entities;
using Prost.Api.Domain.ValueObjects;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Posts;

internal sealed class CreatePostHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<CreatePostCommand, ErrorOr<CreatePostResponse>>
{
    public async ValueTask<ErrorOr<CreatePostResponse>> Handle(
        CreatePostCommand command,
        CancellationToken cancellationToken)
    {
        var errors = new List<Error>();

        var title = Title.Create(command.Title);

        if (title.IsError) errors.AddRange(title.Errors);

        var slug = Slug.Create(command.Slug);

        if (slug.IsError) errors.AddRange(slug.Errors);

        var body = Body.Create(command.Body);

        if (body.IsError) errors.AddRange(body.Errors);

        if (errors.Count > 0) return errors;

        var post = Post.Create(
            PostId.Create(),
            command.AccountId,
            title.Value,
            slug.Value,
            body.Value);

        context.Posts.Add(post);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintException ex) when (ex.RefersTo<Slug>())
        {
            return CreatePostErrors.SlugTaken;
        }
        await cache.RemoveByTagAsync(
            $"accounts:{command.AccountId}:posts:pages",
            token: cancellationToken);

        return new CreatePostResponse(post.Id.Value);
    }
}
