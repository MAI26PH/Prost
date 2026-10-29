using Mediator;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;

namespace Prost.Api.Features.Authentication;

internal sealed class SignOutEndpoint :
    IEndpoint<AuthenticationEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(
                "sign-out",
                async (
                    SignOutRequest request,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new SignOutCommand(request.RefreshToken),
                        cancellationToken);

                    return result.Match(
                        _ => Results.NoContent(),
                        ErrorResults.Problem);
                })
            .WithName("SignOut");
    }
}
