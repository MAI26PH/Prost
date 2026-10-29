namespace Prost.Api.Features.Authentication;

internal sealed record SignUpRequest(
    string Username,
    string Email,
    string Password);
