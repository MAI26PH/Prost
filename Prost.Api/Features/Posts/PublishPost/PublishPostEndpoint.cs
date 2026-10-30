using Mediator;
using Prost.Api.Common.Claims;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;
using Prost.Api.Domain.Entities;
using System.Security.Claims;

namespace Prost.Api.Features.Posts;

internal sealed class PublishPostEndpoint :
    IEndpoint<PostsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(
                "{id:guid}/publish",
                async (
                    Guid id,
                    ClaimsPrincipal principal,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new PublishPostCommand(
                            principal.AccountId,
                            PostId.From(id)),
                        cancellationToken);

                    return result.Match(
                        response => Results.CreatedAtRoute(
                            "GetPublishedPost",
                            new { slug = response.Slug },
                            response),
                        ErrorResults.Problem);
                })
            .RequireAuthorization()
            .WithName("PublishPost");
    }
}
