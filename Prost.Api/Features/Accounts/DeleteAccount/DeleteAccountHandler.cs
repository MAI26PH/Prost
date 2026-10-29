using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Accounts;

internal sealed class DeleteAccountHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<DeleteAccountCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(
        DeleteAccountCommand command,
        CancellationToken cancellationToken)
    {
        await context.Accounts
            .Where(x => x.Id == command.AccountId)
            .ExecuteDeleteAsync(cancellationToken);

        await cache.RemoveByTagAsync(
            $"accounts:{command.AccountId}",
            token: cancellationToken);

        return Result.Success;
    }
}
