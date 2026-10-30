using ErrorOr;
using Mediator;
using Prost.Api.Domain.Entities;

namespace Prost.Api.Features.Posts;

internal sealed record PublishPostCommand(
    AccountId AccountId,
    PostId PostId
) : IRequest<ErrorOr<PublishPostResponse>>;
