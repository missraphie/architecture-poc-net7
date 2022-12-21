using Xacte.Patient.Data.Tests.Fixtures;
using Xunit;

namespace Xacte.Patient.Data.Tests.Collections
{
    [CollectionDefinition(nameof(MsSqlDatabaseCollection))]
    public sealed class MsSqlDatabaseCollection : ICollectionFixture<MsSqlLocalDatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces
    }
}
