using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Common.Pagination;
using Prost.Api.Data;
using Prost.Api.Data.Pagination;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.PublishedPosts;

internal sealed class GetPublishedPostsHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<GetPublishedPostsQuery, ErrorOr<CursorResponse>>
{
    public async ValueTask<ErrorOr<CursorResponse>> Handle(
        GetPublishedPostsQuery query,
        CancellationToken cancellationToken)
    {
        return await cache.GetOrSetAsync(
            $"published-posts:pages:{query.Page}",
            ct => context.PublishedPosts
                .AsNoTracking()
                .Include(x => x.Account)
                .ToCursorResponseAsync(
                    query.Page,
                    mapper: x => new
                    {
                        Slug = x.Slug.Value,
                        Title = x.Title.Value,
                        Body = x.Body.Value,
                        PublishedBy = x.Account.Username.Value,
                        x.PublishedAt,
                        x.UpdatedAt
                    },
                    selector: x => x.UpdatedOrPublishedAt,
                    tiebreaker: x => x.Id,
                    ct),
            tags:
            [
                "published-posts",
                "published-posts:pages"
            ],
            token: cancellationToken);
    }
}
