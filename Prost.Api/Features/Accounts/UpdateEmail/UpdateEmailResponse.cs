namespace Prost.Api.Features.Accounts;

internal sealed record UpdateEmailResponse(
    string AccessToken,
    string RefreshToken);
