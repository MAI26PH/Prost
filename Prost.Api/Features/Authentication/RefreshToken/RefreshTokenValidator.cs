using FluentValidation;

namespace Prost.Api.Features.Authentication;

internal sealed class RefreshTokenValidator :
    AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
