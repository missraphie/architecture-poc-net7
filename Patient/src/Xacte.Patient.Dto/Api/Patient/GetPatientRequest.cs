using System.ComponentModel.DataAnnotations;

namespace Xacte.Patient.Dto.Api.Patient
{
    public sealed class GetPatientRequest
    {
        public GetPatientRequest(Guid guid)
        {
            Guid = guid;
        }

        [Required]
        public Guid Guid { get; set; }
    }
}
