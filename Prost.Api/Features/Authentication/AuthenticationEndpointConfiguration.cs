using Prost.Api.Common.Routing;

namespace Prost.Api.Features.Authentication;

internal sealed class AuthenticationEndpointConfiguration :
    EndpointConfiguration<AuthenticationEndpointConfiguration>
{
    public override IEndpointRouteBuilder Configure(
        IEndpointRouteBuilder builder)
    {
        return builder
            .MapGroup("authentication")
            .WithTags("Authentication")
            .HasApiVersion(1);
    }
}
