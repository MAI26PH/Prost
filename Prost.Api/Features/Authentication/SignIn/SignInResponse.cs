namespace Prost.Api.Features.Authentication;

internal sealed record SignInResponse(
    string AccessToken,
    string RefreshToken);
