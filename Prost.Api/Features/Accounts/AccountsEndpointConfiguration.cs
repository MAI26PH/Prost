using Prost.Api.Common.Routing;

namespace Prost.Api.Features.Accounts;

internal sealed class AccountsEndpointConfiguration :
    EndpointConfiguration<AccountsEndpointConfiguration>
{
    public override IEndpointRouteBuilder Configure(
        IEndpointRouteBuilder builder)
    {
        return builder
            .MapGroup("accounts")
            .WithTags("Accounts")
            .HasApiVersion(1);
    }
}
