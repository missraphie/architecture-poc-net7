using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Xacte.Common.Data
{
    public abstract class XacteDbContext : DbContext
    {
        private IDbContextTransaction? _transaction;

        protected XacteDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await Database.BeginTransactionAsync(cancellationToken);
        }

        public int Commit()
        {
            ArgumentNullException.ThrowIfNull(_transaction);
            try
            {
                var result = base.SaveChanges();
                _transaction.Commit();
                return result;
            }
            catch
            {
                Rollback();
                throw;
            }
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(_transaction);
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                await _transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await RollbackAsync(cancellationToken);
                throw;
            }
        }

        public void Rollback()
        {
            ArgumentNullException.ThrowIfNull(_transaction);
            _transaction.Rollback();
            _transaction.Dispose();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(_transaction);
            await _transaction.RollbackAsync(cancellationToken);
            _transaction.Dispose();
        }

        public async Task<int> RunAsTransactionAsync(Func<Task> actionAsync, Func<Exception, Task>? catchActionAsync, CancellationToken cancellationToken = default)
        {
            // Begin Transaction
            IDbContextTransaction transaction = await Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Run actions
                await actionAsync();

                // Commit
                var result = await base.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch (Exception e)
            {
                // If there's a custom catch action, run it
                if (catchActionAsync is not null)
                {
                    await catchActionAsync(e);
                }
                // If an exception ocurred, rollback everything
                await transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await transaction.DisposeAsync();
            }
            // If it gets here, it means the transaction failed, therefore no rolls were affected
            return 0;
        }
    }
}
