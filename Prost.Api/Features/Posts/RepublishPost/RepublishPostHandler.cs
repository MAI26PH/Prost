using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Posts;

internal sealed class RepublishPostHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<RepublishPostCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(
        RepublishPostCommand command,
        CancellationToken cancellationToken)
    {
        var post = await context.Posts
            .Include(x => x.PublishedPost)
            .Where(x =>
                x.Id == command.PostId &&
                x.AccountId == command.AccountId)
            .SingleOrDefaultAsync(cancellationToken);

        if (post is null) return RepublishPostErrors.NotFound;

        if (post.PublishedPost is not { } publishedPost)
        {
            return RepublishPostErrors.NotPublished;
        }
        publishedPost.UpdateTitle(post.Title);

        publishedPost.UpdateBody(post.Body);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return RepublishPostErrors.ConcurrencyConflict;
        }
        await cache.RemoveByTagAsync(
            $"published-posts:{command.PostId}",
            token: cancellationToken);

        await cache.RemoveByTagAsync(
            "published-posts:pages",
            token: cancellationToken);

        return Result.Success;
    }
}
