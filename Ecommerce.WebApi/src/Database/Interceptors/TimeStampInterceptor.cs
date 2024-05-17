using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.WebApi.src.Database
{
    public class TimestampInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var addedEntries = eventData.Context!
            .ChangeTracker.Entries().Where(e => e.State == EntityState.Added);

            var updatedEntries = eventData.Context
            .ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);

            foreach (var entry in addedEntries)
            {
                if (entry.Entity is TimeStamp coreEntity)
                {
                    coreEntity.CreatedAt = DateTime.Now;
                    coreEntity.UpdatedAt = DateTime.Now;
                    Console.WriteLine(coreEntity.CreatedAt.ToString());
                }
            }

            foreach (var entry in updatedEntries)
            {
                if (entry.Entity is TimeStamp coreEntity)
                {
                    coreEntity.UpdatedAt = DateTime.Now;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}