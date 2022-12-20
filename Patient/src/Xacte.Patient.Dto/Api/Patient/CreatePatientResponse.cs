using Xacte.Common.Responses;

namespace Xacte.Patient.Dto.Api.Patient
{
    public sealed class CreatePatientResponse : XacteResponse<PatientResponse>
    {
        public override IList<PatientResponse> Data { get; set; } = Array.Empty<PatientResponse>();
    }
}
