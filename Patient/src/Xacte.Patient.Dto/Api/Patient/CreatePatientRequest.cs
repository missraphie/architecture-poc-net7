using System.ComponentModel.DataAnnotations;

namespace Xacte.Patient.Dto.Api.Patient
{
    public sealed class CreatePatientRequest
    {
        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;
    }
}
