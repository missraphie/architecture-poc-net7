using Xacte.Patient.Data.Tests.Configurations;
using Xunit;

namespace Xacte.Patient.Data.Tests.Fixtures
{
    internal sealed class MsSqlLocalDatabaseFixture : IAsyncLifetime
    {
        public async Task InitializeAsync()
        {
            await DbContextService.InitializeDatabaseForTesting();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
