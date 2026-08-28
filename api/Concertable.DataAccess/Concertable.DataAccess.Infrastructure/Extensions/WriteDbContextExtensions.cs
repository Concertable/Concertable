using Concertable.DataAccess.Application;
using Microsoft.EntityFrameworkCore;

namespace Concertable.DataAccess.Infrastructure.Extensions;

public static class WriteDbContextExtensions
{
    extension(IWriteDbContext context)
    {
        /// <summary>Saves pending changes. A concurrency conflict returns <c>false</c> and clears the complete
        /// change tracker; every other failure propagates.</summary>
        public async Task<bool> TrySaveChangesAsync(CancellationToken ct = default)
        {
            try
            {
                await context.SaveChangesAsync(ct);
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.DiscardFailedChanges();
                return false;
            }
        }
    }
}
