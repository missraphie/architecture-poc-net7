using Microsoft.EntityFrameworkCore;
using Xacte.Patient.Business.Tests.MockData;
using Xacte.Patient.Data.Contexts;

namespace Xacte.Patient.Data.Tests.Configurations
{
    internal static class DbContextFactory
    {
        /// <summary>
        /// Creates a DB Context
        /// </summary>
        /// <returns>Return the context created</returns>
        internal static PatientContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PatientContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Xacte.Patient.Testing;Trusted_Connection=True;");
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();

            return new PatientContext(optionsBuilder.Options, CurrentUserServiceMock.GetCurrentUserService());
        }
    }
}
