namespace Prost.Api.Common.Pagination;

internal sealed record CursorResponse(
    IReadOnlyList<dynamic> Items,
    string? NextCursor,
    bool HasNext);
