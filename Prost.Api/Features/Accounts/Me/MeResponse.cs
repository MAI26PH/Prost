namespace Prost.Api.Features.Accounts;

internal sealed record class MeResponse(
    Guid Id,
    string Username,
    string Email,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
