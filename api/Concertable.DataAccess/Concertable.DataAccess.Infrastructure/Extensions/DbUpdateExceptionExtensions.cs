using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Concertable.DataAccess.Infrastructure.Extensions;

public static class DbUpdateExceptionExtensions
{
    public static bool IsDuplicateKey(this DbUpdateException ex) =>
        ex.InnerException is SqlException sqlEx && sqlEx.IsDuplicateKey();

    public static void DiscardFailedChanges(this DbUpdateException ex)
    {
        foreach (var context in ex.Entries.Select(entry => entry.Context).Distinct())
            context.ChangeTracker.Clear();
    }
}
