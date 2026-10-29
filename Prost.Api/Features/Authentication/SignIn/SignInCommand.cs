using ErrorOr;
using Mediator;

namespace Prost.Api.Features.Authentication;

internal sealed record SignInCommand(
    string Email,
    string Password
) : IRequest<ErrorOr<SignInResponse>>;
