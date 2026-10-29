using Mediator;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;

namespace Prost.Api.Features.Authentication;

internal sealed class RefreshTokenEndpoint :
    IEndpoint<AuthenticationEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(
                "refresh-token",
                async (
                    RefreshTokenRequest request,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new RefreshTokenCommand(request.RefreshToken),
                        cancellationToken);

                    return result.Match(
                        Results.Ok,
                        ErrorResults.Problem);
                })
            .WithName("RefreshToken");
    }
}
