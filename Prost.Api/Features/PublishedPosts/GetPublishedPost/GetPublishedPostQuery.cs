using ErrorOr;
using Mediator;

namespace Prost.Api.Features.PublishedPosts;

internal sealed record GetPublishedPostQuery(
    string Slug
) : IRequest<ErrorOr<GetPublishedPostResponse>>;
