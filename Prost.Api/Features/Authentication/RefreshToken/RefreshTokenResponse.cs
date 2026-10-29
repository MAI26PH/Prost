namespace Prost.Api.Features.Authentication;

internal sealed record RefreshTokenResponse(
   string AccessToken,
   string RefreshToken);
