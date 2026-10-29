using Prost.Api.Common.DependencyInjection;

namespace Prost.Api.Common.Routing;

internal abstract class EndpointConfiguration
{
    public abstract void MapEndpointConfiguration(
        IApplicationBuilder app,
        IEndpointRouteBuilder builder);
}

internal abstract class EndpointConfiguration<TSelf> : EndpointConfiguration
    where TSelf : EndpointConfiguration<TSelf>
{
    public abstract IEndpointRouteBuilder Configure(
        IEndpointRouteBuilder builder);

    public override void MapEndpointConfiguration(
        IApplicationBuilder app,
        IEndpointRouteBuilder builder)
    {
        app.MapEndpointsFromConfiguration<TSelf>(Configure(builder));
    }
}
