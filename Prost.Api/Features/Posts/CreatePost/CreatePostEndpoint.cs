using Mediator;
using Microsoft.AspNetCore.Mvc;
using Prost.Api.Common.Claims;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;
using System.Security.Claims;

namespace Prost.Api.Features.Posts;

internal sealed class CreatePostEndpoint :
    IEndpoint<PostsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(
                string.Empty,
                async (
                    CreatePostRequest request,
                    ClaimsPrincipal principal,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new CreatePostCommand(
                            principal.AccountId,
                            request.Title,
                            request.Slug,
                            request.Body),
                        cancellationToken);

                    return result.Match(
                        response => Results.CreatedAtRoute(
                            "GetPost",
                            new { id = response.Id },
                            response),
                        ErrorResults.Problem);
                })
            .WithMetadata(new RequestSizeLimitAttribute(1024 * 1024))
            .RequireAuthorization()
            .WithName("CreatePost");
    }
}
