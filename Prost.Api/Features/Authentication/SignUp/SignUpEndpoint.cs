using Mediator;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;

namespace Prost.Api.Features.Authentication;

internal sealed class SignUpEndpoint :
    IEndpoint<AuthenticationEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(
                "sign-up",
                async (
                    SignUpRequest request,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new SignUpCommand(
                            request.Username,
                            request.Email,
                            request.Password),
                        cancellationToken);

                    return result.Match(
                        Results.Ok,
                        ErrorResults.Problem);
                })
            .WithName("SignUp");
    }
}
