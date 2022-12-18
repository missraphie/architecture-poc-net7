using Xacte.Common.Response;

namespace Xacte.Patient.Dto.Api.Patient
{
    public sealed class GetPatientsResponse : XacteResponse<IList<PatientResponse>>
    {
        public override IList<PatientResponse> Data { get; set; } = Array.Empty<PatientResponse>();
    }
}
