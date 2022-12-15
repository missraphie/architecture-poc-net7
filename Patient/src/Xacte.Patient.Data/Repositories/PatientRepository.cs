using Xacte.Patient.Data.Contexts;

namespace Xacte.Patient.Data.Repositories
{
    public sealed class PatientRepository
    {
        private readonly PatientDbContext _context;

        public PatientRepository(PatientDbContext context)
        {
            _context = context;
        }
    }
}
