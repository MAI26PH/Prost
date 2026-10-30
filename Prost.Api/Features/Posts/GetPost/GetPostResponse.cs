using System.Text.Json;

namespace Prost.Api.Features.Posts;

internal sealed record GetPostResponse(
    Guid Id,
    string Title,
    string Slug,
    JsonElement Body,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
