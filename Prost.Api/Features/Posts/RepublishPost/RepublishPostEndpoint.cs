using Mediator;
using Prost.Api.Common.Claims;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;
using Prost.Api.Domain.Entities;
using System.Security.Claims;

namespace Prost.Api.Features.Posts;

internal sealed class RepublishPostEndpoint :
    IEndpoint<PostsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(
                "{id:guid}/republish",
                async (
                    Guid id,
                    ClaimsPrincipal principal,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new RepublishPostCommand(
                            principal.AccountId,
                            PostId.From(id)),
                        cancellationToken);

                    return result.Match(
                        _ => Results.NoContent(),
                        ErrorResults.Problem);
                })
            .RequireAuthorization()
            .WithName("RepublishPost");
    }
}
