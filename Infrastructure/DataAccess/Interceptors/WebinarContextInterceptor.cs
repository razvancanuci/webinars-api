using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataAccess.Interceptors;

[ExcludeFromCodeCoverage]
public class WebinarContextInterceptor: SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditableEntities(DbContext context)
    {
        DateTime utcNow = DateTime.UtcNow;
        var entities = context.ChangeTracker.Entries<Webinar>().ToList();

        foreach (var entry in entities)
        {
            if (entry.State == EntityState.Added)
            {
                SetCurrentPropertyValue(entry, nameof(Webinar.CreatedAt), utcNow);
            }

            if (entry.State == EntityState.Deleted)
            {
                SetCurrentPropertyValue(entry, nameof(Webinar.IsDeleted), true);
                entry.State = EntityState.Modified;
            }
        }
    }
    
    static void SetCurrentPropertyValue<T>(
        EntityEntry entry,
        string propertyName,
        T value) =>
        entry.Property(propertyName).CurrentValue = value;
}