using ErrorOr;
using Mediator;
using Prost.Api.Domain.Entities;
using System.Text.Json;

namespace Prost.Api.Features.Posts;

internal sealed record CreatePostCommand(
    AccountId AccountId,
    string Title,
    string Slug,
    JsonElement Body
) : IRequest<ErrorOr<CreatePostResponse>>;
