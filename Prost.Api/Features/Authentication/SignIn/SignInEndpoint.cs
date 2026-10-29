using Mediator;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;

namespace Prost.Api.Features.Authentication;

internal sealed class SignInEndpoint :
    IEndpoint<AuthenticationEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(
                "sign-in",
                async (
                    SignInRequest request,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new SignInCommand(
                            request.Email,
                            request.Password),
                        cancellationToken);

                    return result.Match(
                        Results.Ok,
                        ErrorResults.Problem);
                })
            .WithName("SignIn");
    }
}
