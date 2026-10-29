using FluentValidation;
using Mediator;
using Prost.Api.Common.Claims;
using Prost.Api.Common.Http;
using Prost.Api.Common.Routing;
using System.Security.Claims;

namespace Prost.Api.Features.Accounts;

internal sealed class UpdateEmailEndpoint :
    IEndpoint<AccountsEndpointConfiguration>
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder
            .MapPatch(
                "email",
                async (
                    UpdateEmailRequest request,
                    ClaimsPrincipal principal,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new UpdateEmailCommand(
                            principal.AccountId,
                            request.Email),
                        cancellationToken);

                    return result.Match(
                        _ => Results.NoContent(),
                        ErrorResults.Problem);
                })
            .RequireAuthorization()
            .WithName("UpdateEmail");
    }
}
