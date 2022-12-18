using Xacte.Common.Response;

namespace Xacte.Patient.Dto.Api.Patient
{

    public sealed class GetPatientResponse : XacteResponse<PatientResponse>
    {
        public override PatientResponse Data { get; set; } = new PatientResponse();
    }
}
