using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using Prost.Api.Domain.ValueObjects;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.PublishedPosts;

internal sealed class GetPublishedPostHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<GetPublishedPostQuery, ErrorOr<GetPublishedPostResponse>>
{
    public async ValueTask<ErrorOr<GetPublishedPostResponse>> Handle(
        GetPublishedPostQuery query,
        CancellationToken cancellationToken)
    {
        var slug = Slug.Create(query.Slug);

        if (slug.IsError) return slug.Errors;

        var response = await cache.GetOrSetAsync<GetPublishedPostResponse?>(
            $"published-posts:{slug.Value}",
            async (ctx, ct) =>
            {
                var result = await context.PublishedPosts
                    .AsNoTracking()
                    .Where(x => x.Slug == slug.Value)
                    .Select(x => new
                    {
                        x.PostId,
                        Response = new GetPublishedPostResponse(
                            Title: x.Title.Value,
                            Body: x.Body.Value,
                            PublishedBy: x.Account.Username.Value,
                            PublishedAt: x.PublishedAt,
                            UpdatedAt: x.UpdatedAt)
                    })
                    .SingleOrDefaultAsync(ct);

                if (result is null)
                {
                    ctx.Tags =
                    [
                        "published-posts",
                        $"published-posts:{slug.Value}"
                    ];
                    return null;
                }
                ctx.Tags =
                [
                    "published-posts",
                    $"published-posts:{result.PostId}"
                ];
                return result.Response;
            },
            token: cancellationToken);

        return response switch
        {
            not null => response,
            null => GetPublishedPostErrors.NotFound,
        };
    }
}
