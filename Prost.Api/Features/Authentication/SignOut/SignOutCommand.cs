using ErrorOr;
using Mediator;

namespace Prost.Api.Features.Authentication;

internal sealed record SignOutCommand(
    string RefreshToken
) : IRequest<ErrorOr<Success>>;
