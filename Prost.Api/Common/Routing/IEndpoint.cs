namespace Prost.Api.Common.Routing;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder builder);
}

public interface IEndpoint<TEndpointConfiguration> :
    IEndpoint
    where TEndpointConfiguration : EndpointConfiguration;
