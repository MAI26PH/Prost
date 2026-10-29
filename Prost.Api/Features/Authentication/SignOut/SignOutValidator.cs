using FluentValidation;

namespace Prost.Api.Features.Authentication;

internal sealed class SignOutValidator :
    AbstractValidator<SignOutCommand>
{
    public SignOutValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
