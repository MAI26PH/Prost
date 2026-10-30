using ErrorOr;
using Mediator;
using Prost.Api.Common.Pagination;
using Prost.Api.Domain.Entities;

namespace Prost.Api.Features.Posts;

internal sealed record GetPostsQuery(
    AccountId AccountId,
    CursorPage Page
) : IRequest<ErrorOr<CursorResponse>>;
