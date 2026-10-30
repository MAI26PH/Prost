using Mediator;
using Prost.Api.Common.Claims;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;
using Prost.Api.Domain.Entities;
using System.Security.Claims;

namespace Prost.Api.Features.Posts;

internal sealed class GetPostEndpoint :
    IEndpoint<PostsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(
                "{id:guid}",
                async (
                    Guid id,
                    ClaimsPrincipal principal,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new GetPostQuery(
                            principal.AccountId,
                            PostId.From(id)),
                        cancellationToken);

                    return result.Match(
                        Results.Ok,
                        ErrorResults.Problem);
                })
            .RequireAuthorization()
            .WithName("GetPost");
    }
}
