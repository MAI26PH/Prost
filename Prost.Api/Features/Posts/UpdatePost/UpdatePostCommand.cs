using ErrorOr;
using Mediator;
using Prost.Api.Domain.Entities;
using System.Text.Json;

namespace Prost.Api.Features.Posts;

internal sealed record UpdatePostCommand(
    AccountId AccountId,
    PostId PostId,
    string Title,
    JsonElement Body
) : IRequest<ErrorOr<Success>>;
