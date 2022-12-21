using Microsoft.EntityFrameworkCore;

namespace Xacte.Patient.Data.Tests.Configurations
{
    internal static class DbContextService
    {
        /// <summary>
        /// Makes sure the database is initialized properly. DB will be empty with migrations executed.
        /// </summary>
        internal static async Task InitializeDatabaseForTesting()
        {
            var context = DbContextFactory.CreateDbContext();

            // We could delete the database after the tests... But keeping it makes it easier to see
            // the records in case we need it. So we are purposively deleting it right after creation only
            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
        }
    }
}
