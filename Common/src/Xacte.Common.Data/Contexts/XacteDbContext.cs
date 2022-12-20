using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Xacte.Common.Data.Entities;
using Xacte.Common.Services;

namespace Xacte.Common.Data.Contexts
{
    public abstract class XacteDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private IDbContextTransaction? _transaction;

        protected XacteDbContext(DbContextOptions options, ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
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

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditInformation();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateAuditInformation();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
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

        private void UpdateAuditInformation()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries()
                .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified)
                .Where(entry => entry.Entity is Entity /*|| entry.Entity is NoSqlEntity*/);

            foreach (EntityEntry entry in entries)
            {
                DateTime now = DateTime.UtcNow;

                entry.Property(nameof(Entity.ModifiedBy)).CurrentValue = _currentUserService.Id;
                entry.Property(nameof(Entity.ModifiedByName)).CurrentValue = _currentUserService.Name;
                entry.Property(nameof(Entity.ModifiedOn)).CurrentValue = now;

                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(Entity.CreatedBy)).CurrentValue = _currentUserService.Id;
                    entry.Property(nameof(Entity.CreatedByName)).CurrentValue = _currentUserService.Name;
                    entry.Property(nameof(Entity.CreatedOn)).CurrentValue = now;
                }
            }
        }

    }
}
