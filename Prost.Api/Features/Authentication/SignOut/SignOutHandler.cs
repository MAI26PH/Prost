using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using Prost.Api.Domain.ValueObjects;
using Prost.Api.Features.Authentication.Services;

namespace Prost.Api.Features.Authentication;

internal sealed class SignOutHandler(
    ProstDbContext context,
    TimeProvider time,
    ITokenHasher tokenHasher,
    ITokenFamilyRevocation tokenFamilyRevocation
) : IRequestHandler<SignOutCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(
        SignOutCommand command,
        CancellationToken cancellationToken)
    {
        var tokenHash = TokenHash.Create(
            tokenHasher.Hash(command.RefreshToken));

        if (tokenHash.IsError) return tokenHash.Errors;

        var refreshToken = await context.RefreshTokens
            .Include(x => x.TokenFamily)
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash.Value,
                cancellationToken);

        if (refreshToken is not { TokenFamily: { } tokenFamily })
        {
            return Result.Success;
        }
        tokenFamily.Revoke(time.GetUtcNow());

        await context.SaveChangesAsync(cancellationToken);

        await tokenFamilyRevocation.RevokeAsync(
            tokenFamily.Id,
            cancellationToken);

        return Result.Success;
    }
}
