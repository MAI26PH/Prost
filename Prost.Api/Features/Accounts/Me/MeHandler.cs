using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Accounts;

internal sealed class MeHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<MeQuery, ErrorOr<MeResponse>>
{
    public async ValueTask<ErrorOr<MeResponse>> Handle(
        MeQuery query,
        CancellationToken cancellationToken)
    {
        var response = await cache.GetOrSetAsync(
            $"accounts:{query.AccountId}",
            ct => context.Accounts
                .AsNoTracking()
                .Where(x => x.Id == query.AccountId)
                .Select(x => new MeResponse(
                    Id: x.Id.Value,
                    Username: x.Username.Value,
                    Email: x.Email.Value,
                    CreatedAt: x.CreatedAt,
                    UpdatedAt: x.UpdatedAt))
                .SingleOrDefaultAsync(ct),
            tags: [$"accounts:{query.AccountId}"],
            token: cancellationToken);

        return response switch
        {
            not null => response,
            null => MeErrors.NotFound
        };
    }
}
