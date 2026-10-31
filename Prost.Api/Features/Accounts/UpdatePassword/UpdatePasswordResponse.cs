namespace Prost.Api.Features.Accounts;

internal sealed record UpdatePasswordResponse(
    string AccessToken,
    string RefreshToken);
