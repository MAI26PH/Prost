using ErrorOr;
using Mediator;

namespace Prost.Api.Features.Authentication;

internal sealed record SignUpCommand(
    string Username,
    string Email,
    string Password
) : IRequest<ErrorOr<SignUpResponse>>;
