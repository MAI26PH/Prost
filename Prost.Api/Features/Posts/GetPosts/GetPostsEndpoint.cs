using Mediator;
using Prost.Api.Common.Claims;
using Prost.Api.Common.Http;
using Prost.Api.Common.Pagination;
using Prost.Api.Common.Routing;
using System.Security.Claims;

namespace Prost.Api.Features.Posts;

internal sealed class GetPostsEndpoint :
    IEndpoint<PostsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(
                string.Empty,
                async (
                    [AsParameters] CursorPage page,
                    ClaimsPrincipal principal,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new GetPostsQuery(
                            principal.AccountId,
                            page),
                        cancellationToken);

                    return result.Match(
                        Results.Ok,
                        ErrorResults.Problem);
                })
            .RequireAuthorization()
            .WithName("GetPosts");
    }
}
