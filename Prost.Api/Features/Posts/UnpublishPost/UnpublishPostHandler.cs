using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Posts;

internal sealed class UnpublishPostHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<UnpublishPostCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(
        UnpublishPostCommand command,
        CancellationToken cancellationToken)
    {
        await context.PublishedPosts
            .Where(x =>
                x.PostId == command.PostId &&
                x.AccountId == command.AccountId)
            .ExecuteDeleteAsync(cancellationToken);

        await cache.RemoveByTagAsync(
            $"published-posts:{command.PostId}",
            token: cancellationToken);

        await cache.RemoveByTagAsync(
            "published-posts:pages",
            token: cancellationToken);

        return Result.Success;
    }
}
