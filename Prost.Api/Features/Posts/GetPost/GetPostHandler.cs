using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Posts;

internal sealed class GetPostHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<GetPostQuery, ErrorOr<GetPostResponse>>
{
    public async ValueTask<ErrorOr<GetPostResponse>> Handle(
        GetPostQuery query,
        CancellationToken cancellationToken)
    {
        var response = await cache.GetOrSetAsync(
            $"accounts:{query.AccountId}:posts:{query.PostId}",
            ct => context.Posts
                .AsNoTracking()
                .Where(x =>
                    x.Id == query.PostId &&
                    x.AccountId == query.AccountId)
                .Select(x => new GetPostResponse(
                    Id: x.Id.Value,
                    Title: x.Title.Value,
                    Slug: x.Slug.Value,
                    Body: x.Body.Value,
                    CreatedAt: x.CreatedAt,
                    UpdatedAt: x.UpdatedAt))
                .SingleOrDefaultAsync(ct),
            tags:
            [
                $"accounts:{query.AccountId}",
                $"accounts:{query.AccountId}:posts",
                $"accounts:{query.AccountId}:posts:{query.PostId}"
            ],
            token: cancellationToken);

        return response switch
        {
            not null => response,
            null => GetPostErrors.NotFound
        };
    }
}
