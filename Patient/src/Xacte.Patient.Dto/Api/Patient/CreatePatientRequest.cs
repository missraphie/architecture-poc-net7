using System.ComponentModel.DataAnnotations;

namespace Xacte.Patient.Dto.Api.Patient
{
    public sealed class CreatePatientRequest
    {
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceName = nameof(ModelValidation.FirstNameRequired),
            ErrorMessageResourceType = typeof(ModelValidation))]
        [StringLength(10, MinimumLength = 3,
            ErrorMessageResourceName = nameof(ModelValidation.FirstNameRange),
            ErrorMessageResourceType = typeof(ModelValidation))]
        public string FirstName { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceName = nameof(ModelValidation.LastNameRequired),
            ErrorMessageResourceType = typeof(ModelValidation))]
        public string LastName { get; set; } = string.Empty;
    }
}
