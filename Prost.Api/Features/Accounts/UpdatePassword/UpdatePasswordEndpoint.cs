using FluentValidation;
using Mediator;
using Prost.Api.Common.Claims;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;
using System.Security.Claims;

namespace Prost.Api.Features.Accounts;

internal sealed class UpdatePasswordEndpoint :
    IEndpoint<AccountsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPatch(
                "password",
                async (
                    UpdatePasswordRequest request,
                    ClaimsPrincipal principal,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new UpdatePasswordCommand(
                            principal.AccountId,
                            request.Password),
                        cancellationToken);

                    return result.Match(
                        Results.Ok,
                        ErrorResults.Problem);
                })
            .RequireAuthorization()
            .WithName("UpdatePassword");
    }
}
