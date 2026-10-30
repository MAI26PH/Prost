using ErrorOr;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Prost.Api.Data;
using Prost.Api.Domain.ValueObjects;
using ZiggyCreatures.Caching.Fusion;

namespace Prost.Api.Features.Posts;

internal sealed class UpdatePostHandler(
    ProstDbContext context,
    IFusionCache cache
) : IRequestHandler<UpdatePostCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(
        UpdatePostCommand command,
        CancellationToken cancellationToken)
    {
        var errors = new List<Error>();

        var title = Title.Create(command.Title);

        if (title.IsError) errors.AddRange(title.Errors);

        var body = Body.Create(command.Body);

        if (body.IsError) errors.AddRange(body.Errors);

        if (errors.Count > 0) return errors;

        var post = await context.Posts
            .Where(x =>
                x.Id == command.PostId &&
                x.AccountId == command.AccountId)
            .SingleOrDefaultAsync(cancellationToken);

        if (post is null) return UpdatePostErrors.NotFound;

        post.UpdateTitle(title.Value);

        post.UpdateBody(body.Value);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return UpdatePostErrors.ConcurrencyConflict;
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
