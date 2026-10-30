using ErrorOr;
using Mediator;
using Prost.Api.Domain.Entities;

namespace Prost.Api.Features.Posts;

internal sealed record UnpublishPostCommand(
    AccountId AccountId,
    PostId PostId
) : IRequest<ErrorOr<Success>>;
