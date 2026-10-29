using FluentValidation;
using Mediator;
using Prost.Api.Common.Claims;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;
using System.Security.Claims;

namespace Prost.Api.Features.Accounts;

internal sealed class UpdateUsernameEndpoint :
    IEndpoint<AccountsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPatch(
                "username",
                async (
                    UpdateUsernameRequest request,
                    ClaimsPrincipal principal,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new UpdateUsernameCommand(
                            principal.AccountId,
                            request.Username),
                        cancellationToken);

                    return result.Match(
                        _ => Results.NoContent(),
                        ErrorResults.Problem);
                })
            .RequireAuthorization()
            .WithName("UpdateUsername");
    }
}
