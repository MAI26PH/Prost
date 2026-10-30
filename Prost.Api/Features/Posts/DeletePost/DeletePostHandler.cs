using EntityFramework.Exceptions.Common;
using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Posts;

internal sealed class DeletePostHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<DeletePostCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(
        DeletePostCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            await context.Posts
                .Where(x =>
                    x.Id == command.PostId &&
                    x.AccountId == command.AccountId)
                .ExecuteDeleteAsync(cancellationToken);
        }
        catch (ReferenceConstraintException)
        {
            return DeletePostErrors.Published;
        }
        await cache.RemoveByTagAsync(
            $"accounts:{command.AccountId}:posts:{command.PostId}",
            token: cancellationToken);

        await cache.RemoveByTagAsync(
            $"accounts:{command.AccountId}:posts:pages",
            token: cancellationToken);

        return Result.Success;
    }
}
