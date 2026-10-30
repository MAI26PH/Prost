using Mediator;
using Prost.Api.Common.Http;
using Prost.Api.Common.Pagination;
using Prost.Api.Common.Routing;

namespace Prost.Api.Features.PublishedPosts;

internal sealed class GetPublishedPostsEndpoint :
    IEndpoint<PublishedPostsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(
                string.Empty,
                async (
                    [AsParameters] CursorPage page,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new GetPublishedPostsQuery(page),
                        cancellationToken);

                    return result.Match(
                        Results.Ok,
                        ErrorResults.Problem);
                })
            .WithName("GetPublishedPosts");
    }
}
