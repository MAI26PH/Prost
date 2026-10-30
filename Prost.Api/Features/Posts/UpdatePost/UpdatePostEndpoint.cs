using Mediator;
using Prost.Api.Common.Claims;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;
using Prost.Api.Domain.Entities;
using System.Security.Claims;

namespace Prost.Api.Features.Posts;

internal sealed class UpdatePostEndpoint :
    IEndpoint<PostsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPut(
                "{id:guid}",
                async (
                    Guid id,
                    UpdatePostRequest request,
                    ClaimsPrincipal principal,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new UpdatePostCommand(
                            principal.AccountId,
                            PostId.From(id),
                            request.Title,
                            request.Body),
                        cancellationToken);

                    return result.Match(
                        _ => Results.NoContent(),
                        ErrorResults.Problem);
                })
            .RequireAuthorization()
            .WithName("UpdatePost");
    }
}
