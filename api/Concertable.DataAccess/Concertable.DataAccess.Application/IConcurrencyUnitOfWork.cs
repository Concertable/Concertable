namespace Concertable.DataAccess.Application;

public interface IConcurrencyUnitOfWork<TContext> : IUnitOfWork<TContext>
{
    Task<bool> TrySaveChangesAsync(CancellationToken cancellationToken = default);
}
