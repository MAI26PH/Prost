using Mediator;
using Prost.Api.Common.Claims;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;
using System.Security.Claims;

namespace Prost.Api.Features.Accounts;

internal sealed class MeEndpoint
    : IEndpoint<AccountsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapGet(
                "me",
                async (
                    ClaimsPrincipal principal,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new MeQuery(principal.AccountId),
                        cancellationToken);

                    return result.Match(
                        Results.Ok,
                        ErrorResults.Problem);
                })
            .RequireAuthorization()
            .WithName("Me");
    }
}
