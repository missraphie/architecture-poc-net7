namespace Xacte.Patient.Dto.Api.Patient
{
    public sealed class PatientResponse
    {
        public Guid Guid { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
