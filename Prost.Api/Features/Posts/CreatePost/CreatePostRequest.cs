using System.Text.Json;

namespace Prost.Api.Features.Posts;

internal sealed record CreatePostRequest(
    string Title,
    string Slug,
    JsonElement Body);
