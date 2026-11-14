namespace Prost.Api.Common.Routing;

public abstract class EndpointConfiguration
{
    public abstract void MapEndpointConfiguration(
        IApplicationBuilder app,
        IEndpointRouteBuilder builder);
}

public abstract class EndpointConfiguration<TSelf> :
    EndpointConfiguration
    where TSelf : EndpointConfiguration<TSelf>
{
    public abstract IEndpointRouteBuilder Configure(
        IEndpointRouteBuilder builder);
}
