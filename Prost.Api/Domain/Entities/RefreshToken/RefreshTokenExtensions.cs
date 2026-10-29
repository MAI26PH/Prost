using ErrorOr;

namespace Prost.Api.Domain.Entities;

internal static class RefreshTokenExtensions
{
    extension(ErrorOr<RefreshToken> refreshToken)
    {
        public bool HasConsumedError =>
            refreshToken.Errors
                .Contains(RefreshTokenErrors.Consumed);
    }
}
