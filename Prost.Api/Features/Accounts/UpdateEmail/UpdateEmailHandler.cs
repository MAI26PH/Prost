using EntityFramework.Exceptions.Common;
using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using Prost.Api.Data.Exceptions;
using Prost.Api.Domain.ValueObjects;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Accounts;

internal sealed class UpdateEmailHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<UpdateEmailCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(
        UpdateEmailCommand command,
        CancellationToken cancellationToken)
    {
        var email = Email.Create(command.Email);

        if (email.IsError) return email.Errors;

        if (await context.Accounts.SingleOrDefaultAsync(
            x => x.Id == command.AccountId,
            cancellationToken) is not { } account)
        {
            return UpdateUsernameErrors.NotFound;
        }
        if (account.Email == email.Value)
        {
            return Result.Success;
        }
        if (await context.Accounts.AnyAsync(
            x => x.Email == email.Value,
            cancellationToken))
        {
            return UpdateEmailErrors.Taken;
        }
        account.UpdateEmail(email.Value);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintException ex) when (ex.RefersTo<Email>())
        {
            return UpdateEmailErrors.Taken;
        }
        catch (DbUpdateConcurrencyException)
        {
            return UpdateEmailErrors.ConcurrencyConflict;
        }
        await cache.RemoveByTagAsync(
            $"accounts:{command.AccountId}",
            token: cancellationToken);

        return Result.Success;
    }
}
