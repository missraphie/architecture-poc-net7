namespace Xacte.Patient.Dto.Business
{
    public sealed class CreatePatientRequestModel
    {
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
    }
}
