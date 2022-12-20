using System.ComponentModel.DataAnnotations;

namespace Xacte.Patient.Dto.Api.Patient
{
    public sealed class DeletePatientRequest
    {
        public DeletePatientRequest(Guid guid)
        {
            Guid = guid;
        }

        [Required]
        public Guid Guid { get; set; }
    }
}
