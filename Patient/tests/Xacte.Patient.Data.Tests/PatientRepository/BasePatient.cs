using Xacte.Patient.Business.Tests.MockData;
using Xacte.Patient.Data.Repositories.Interfaces;
using Xacte.Patient.Data.Tests.Configurations;
using Xunit;

namespace Xacte.Patient.Data.Tests.PatientRepository
{
    public class BasePatient : BaseIntegrationTests, IAsyncLifetime
    {
        protected Entities.Patient _patient01;
        protected Entities.Patient _patient02;
        protected Entities.Patient _patient03;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public BasePatient() : base()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeRepository();
        }

        public IPatientRepository Repository { get; set; } = null!;

        protected static Entities.Patient GetPatient() => PatientMock.GetPatient();

        protected void InitializeRepository()
        {
            InitializeContext();
            Repository = new Repositories.PatientRepository(Context);
        }

        public async Task InitializeAsync()
        {
            _patient01 = await Repository.CreateAsync(GetPatient());
            _patient02 = await Repository.CreateAsync(GetPatient());
            _patient03 = await Repository.CreateAsync(GetPatient());
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}