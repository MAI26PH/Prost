using System.Text.Json;

namespace Prost.Api.Features.Posts;

internal sealed record UpdatePostRequest(
    string Title,
    JsonElement Body);
