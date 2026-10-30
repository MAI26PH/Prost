using ErrorOr;
using Mediator;
using Prost.Api.Common.Pagination;

namespace Prost.Api.Features.PublishedPosts;

internal sealed record GetPublishedPostsQuery(
    CursorPage Page
) : IRequest<ErrorOr<CursorResponse>>;
