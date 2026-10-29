using ErrorOr;

namespace Prost.Api.Features.Authentication;

internal static class SignInErrors
{
    public static readonly Error InvalidCredentials = Error.Unauthorized(
        code: "SignIn.InvalidCredentials",
        description: "Invalid account credentials.");
}
