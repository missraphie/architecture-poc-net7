using Xacte.Patient.Dto.Api.Patient;
using Xacte.Patient.Dto.Business;

namespace Xacte.Patient.Business.Services.Interfaces
{
    public interface IPatientService
    {
        Task<CreatePatientResponse> CreateAsync(CreatePatientRequestModel model);
        Task<DeletePatientResponse> DeleteAsync(DeletePatientRequestModel model);
        Task<GetPatientResponse> GetAsync(GetPatientRequestModel model);
        Task<GetPatientsResponse> GetAsync();
    }
}