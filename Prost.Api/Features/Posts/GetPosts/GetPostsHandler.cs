using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Common.Pagination;
using Prost.Api.Data;
using Prost.Api.Data.Pagination;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Posts;

internal sealed class GetPostsHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<GetPostsQuery, ErrorOr<CursorResponse>>
{
    public async ValueTask<ErrorOr<CursorResponse>> Handle(
        GetPostsQuery query,
        CancellationToken cancellationToken)
    {
        return await cache.GetOrSetAsync(
            $"accounts:{query.AccountId}:posts:pages:{query.Page}",
            ct => context.Posts
                .AsNoTracking()
                .Where(x => x.AccountId == query.AccountId)
                .ToCursorResponseAsync(
                    query.Page,
                    mapper: x => new
                    {
                        Id = x.Id.Value,
                        Title = x.Title.Value,
                        Slug = x.Slug.Value,
                        Body = x.Body.Value,
                        x.CreatedAt,
                        x.UpdatedAt
                    },
                    selector: x => x.UpdatedOrCreatedAt,
                    tiebreaker: x => x.Id,
                    ct),
            tags:
            [
                $"accounts:{query.AccountId}",
                $"accounts:{query.AccountId}:posts",
                $"accounts:{query.AccountId}:posts:pages",
            ],
            token: cancellationToken);
    }
}
