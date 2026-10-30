using Prost.Api.Common.Routing;

namespace Prost.Api.Features.PublishedPosts;

internal sealed class PublishedPostsEndpointConfiguration
    : EndpointConfiguration<PublishedPostsEndpointConfiguration>
{
    public override IEndpointRouteBuilder Configure(
        IEndpointRouteBuilder builder)
    {
        return builder
            .MapGroup("published-posts")
            .WithTags("Published Posts")
            .HasApiVersion(1);
    }
}
