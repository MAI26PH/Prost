using ErrorOr;
using Mediator;

namespace Prost.Api.Features.Authentication;

internal sealed record RefreshTokenCommand(
    string RefreshToken
) : IRequest<ErrorOr<RefreshTokenResponse>>;
