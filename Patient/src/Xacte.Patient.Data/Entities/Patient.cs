using Xacte.Common.Data.Entities;

namespace Xacte.Patient.Data.Entities
{
    public sealed class Patient : Entity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
