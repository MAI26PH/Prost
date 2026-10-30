using System.Text.Json;

namespace Prost.Api.Features.PublishedPosts;

internal sealed record GetPublishedPostResponse(
    string Title,
    JsonElement Body,
    string PublishedBy,
    DateTimeOffset PublishedAt,
    DateTimeOffset? UpdatedAt);
