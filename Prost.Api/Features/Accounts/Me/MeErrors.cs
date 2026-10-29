using ErrorOr;

namespace Prost.Api.Features.Accounts;

internal static class MeErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: "Me.NotFound",
        description: "The account was not found.");
}
