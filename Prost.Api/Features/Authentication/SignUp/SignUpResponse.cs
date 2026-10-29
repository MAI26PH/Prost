namespace Prost.Api.Features.Authentication;

internal sealed record SignUpResponse(
    string AccessToken,
    string RefreshToken);
