using EntityFramework.Exceptions.Common;
using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using Prost.Api.Data.Exceptions;
using Prost.Api.Domain.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Posts;

internal sealed class PublishPostHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<PublishPostCommand, ErrorOr<PublishPostResponse>>
{
    public async ValueTask<ErrorOr<PublishPostResponse>> Handle(
        PublishPostCommand command,
        CancellationToken cancellationToken)
    {
        var post = await context.Posts
            .AsNoTracking()
            .Where(x =>
                x.Id == command.PostId &&
                x.AccountId == command.AccountId)
            .SingleOrDefaultAsync(cancellationToken);

        if (post is null) return PublishPostErrors.NotFound;

        var publishedPost = PublishedPost.Create(
            PublishedPostId.Create(),
            post);

        context.PublishedPosts.Add(publishedPost);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintException ex) when (ex.RefersTo<PostId>())
        {
            return PublishPostErrors.Published;
        }
        await cache.RemoveByTagAsync(
            $"published-posts:{publishedPost.Slug}",
            token: cancellationToken);

        await cache.RemoveByTagAsync(
            "published-posts:pages",
            token: cancellationToken);

        return new PublishPostResponse(publishedPost.Slug.Value);
    }
}
