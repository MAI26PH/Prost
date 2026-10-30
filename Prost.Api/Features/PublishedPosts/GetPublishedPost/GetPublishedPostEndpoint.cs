using Mediator;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;

namespace Prost.Api.Features.PublishedPosts;

internal sealed class GetPublishedPostEndpoint :
    IEndpoint<PublishedPostsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(
                "{slug}",
                async (
                    string slug,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new GetPublishedPostQuery(slug),
                        cancellationToken);

                    return result.Match(
                        Results.Ok,
                        ErrorResults.Problem);
                })
            .WithName("GetPublishedPost");
    }
}
