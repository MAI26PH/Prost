using Prost.Api.Common.Routing;

namespace Prost.Api.Features.Posts;

internal sealed class PostsEndpointConfiguration :
    EndpointConfiguration<PostsEndpointConfiguration>
{
    public override IEndpointRouteBuilder Configure(
        IEndpointRouteBuilder builder)
    {
        return builder
            .MapGroup("posts")
            .WithTags("Posts")
            .HasApiVersion(1);
    }
}
