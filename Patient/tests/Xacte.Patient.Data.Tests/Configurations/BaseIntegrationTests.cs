using Xacte.Patient.Data.Contexts;

namespace Xacte.Patient.Data.Tests.Configurations
{
    public class BaseIntegrationTests : IDisposable
    {
        /// <summary>
        /// DB Context. 
        /// Should be initialized before each test, so we can simulate 
        /// the same context isolation we get from the API (a new 
        /// context for each request)
        /// </summary>
        public PatientContext Context { get; set; } = null!;

        public BaseIntegrationTests()
        {
            InitializeContext();
        }

        protected void InitializeContext() => Context = DbContextFactory.CreateDbContext();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
        }
    }
}
