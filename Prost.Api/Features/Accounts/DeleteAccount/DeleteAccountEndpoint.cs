using Mediator;
using Prost.Api.Common.Claims;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;
using System.Security.Claims;

namespace Prost.Api.Features.Accounts;

internal sealed class DeleteAccountEndpoint :
    IEndpoint<AccountsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapDelete(
                string.Empty,
                async (
                    ClaimsPrincipal principal,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new DeleteAccountCommand(principal.AccountId),
                        cancellationToken);

                    return result.Match(
                        _ => Results.NoContent(),
                        ErrorResults.Problem);
                })
            .RequireAuthorization()
            .WithName("DeleteAccount");
    }
}
