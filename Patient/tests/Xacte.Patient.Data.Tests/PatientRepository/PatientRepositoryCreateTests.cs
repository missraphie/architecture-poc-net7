using Xacte.Patient.Data.Tests.Collections;
using Xunit;

namespace Xacte.Patient.Data.Tests.PatientRepository
{
    [Collection(nameof(MsSqlDatabaseCollection))]
    public sealed class PatientRepositoryAnyTests : BasePatient
    {
        [Fact]
        public async Task When_CheckingForAnyGuid_Expect_Success_IfExists()
        {
            // Arrange

            // Act
            var byGuidExists = await Repository.AnyAsync(_patient01.Guid);
            var byGuidNonexistent = await Repository.AnyAsync(Guid.NewGuid());

            // Assert
            Assert.True(byGuidExists);
            Assert.False(byGuidNonexistent);
        }
    }
}