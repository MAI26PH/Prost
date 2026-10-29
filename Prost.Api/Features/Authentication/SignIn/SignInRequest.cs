namespace Prost.Api.Features.Authentication;

internal sealed record SignInRequest(
    string Email,
    string Password);
