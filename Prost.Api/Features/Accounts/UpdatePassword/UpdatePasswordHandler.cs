using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using Prost.Api.Domain.ValueObjects;
using Prost.Api.Features.Authentication.Services;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Accounts;

internal sealed class UpdatePasswordHandler(
    ProstDbContext context,
    IFusionCache cache,
    IPasswordHasher passwordHasher
) : IRequestHandler<UpdatePasswordCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(
        UpdatePasswordCommand command,
        CancellationToken cancellationToken)
    {
        var password = Password.Create(command.Password);

        if (password.IsError) return password.Errors;

        var account = await context.Accounts
            .Include(x => x.PasswordCredential)
            .SingleOrDefaultAsync(
                x => x.Id == command.AccountId,
                cancellationToken);

        if (account is not { PasswordCredential: var passwordCredential })
        {
            return UpdatePasswordErrors.NotFound;
        }
        if (passwordCredential is null)
        {
            return UpdatePasswordErrors.NotSet;
        }
        var passwordHash = PasswordHash.Create(
            passwordHasher.Hash(password.Value.Value));

        if (passwordHash.IsError) return passwordHash.Errors;

        passwordCredential.UpdatePasswordHash(passwordHash.Value);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return UpdatePasswordErrors.ConcurrencyConflict;
        }
        await cache.RemoveByTagAsync(
            $"accounts:{command.AccountId}",
            token: cancellationToken);

        return Result.Success;
    }
}
