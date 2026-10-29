using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Prost.Api.Common.Attributes;
using Prost.Api.Domain.Primitives;
using Prost.Api.Domain.Timestamps;

namespace Prost.Api.Data.Interceptors;

[Service(ServiceLifetime.Singleton)]
internal sealed class TimestampsInterceptor(
    TimeProvider time
) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        SetTimestamps(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SetTimestamps(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SetTimestamps(DbContext? context)
    {
        if (context is null) return;

        var now = time.GetUtcNow();

        foreach (var entry in context.ChangeTracker.Entries<ITimestamp>())
        {
            if (entry.State is EntityState.Added)
            {
                if (entry.Entity is ICreatedAt createdAt)
                {
                    createdAt.Value = now;
                }
            }
            if (entry.State is EntityState.Modified)
            {
                if (entry.Entity is IUpdatedAt updatedAt)
                {
                    updatedAt.Value = now;
                }
            }
        }
    }
}
