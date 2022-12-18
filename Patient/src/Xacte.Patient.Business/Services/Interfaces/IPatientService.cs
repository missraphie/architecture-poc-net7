using Xacte.Patient.Dto.Api.Patient;
using Xacte.Patient.Dto.Business;

namespace Xacte.Patient.Business.Services.Interfaces
{
    public interface IPatientService
    {
        Task<GetPatientResponse> GetPatientAsync(GetPatientRequestModel model);
    }
}