using EntityFramework.Exceptions.Common;
using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using Prost.Api.Data.Exceptions;
using Prost.Api.Domain.ValueObjects;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Accounts;

internal sealed class UpdateUsernameHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<UpdateUsernameCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(
        UpdateUsernameCommand command,
        CancellationToken cancellationToken)
    {
        var username = Username.Create(command.Username);

        if (username.IsError) return username.Errors;

        if (await context.Accounts.SingleOrDefaultAsync(
            x => x.Id == command.AccountId,
            cancellationToken) is not { } account)
        {
            return UpdateUsernameErrors.NotFound;
        }
        if (account.Username == username.Value)
        {
            return Result.Success;
        }
        if (await context.Accounts.AnyAsync(
            x => x.Username == username.Value,
            cancellationToken))
        {
            return UpdateUsernameErrors.Taken;
        }
        account.UpdateUsername(username.Value);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintException ex) when (ex.RefersTo<Username>())
        {
            return UpdateUsernameErrors.Taken;
        }
        catch (DbUpdateConcurrencyException)
        {
            return UpdateUsernameErrors.ConcurrencyConflict;
        }
        await cache.RemoveByTagAsync(
            $"accounts:{command.AccountId}",
            token: cancellationToken);

        return Result.Success;
    }
}
