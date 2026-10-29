using ErrorOr;

namespace Prost.Api.Features.Authentication;

internal static class SignUpErrors
{
    public static readonly Error UsernameTaken = Error.Conflict(
        code: "SignUp.UsernameTaken",
        description: "The username is already taken.");

    public static readonly Error EmailTaken = Error.Conflict(
        code: "SignUp.EmailTaken",
        description: "The email address is already taken.");
}
