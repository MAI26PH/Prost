namespace Prost.Api.Common.Routing;

internal interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder builder);
}

internal interface IEndpoint<TEndpointConfiguration> : IEndpoint
    where TEndpointConfiguration : EndpointConfiguration;
