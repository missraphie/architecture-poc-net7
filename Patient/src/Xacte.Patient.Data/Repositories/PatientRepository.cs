using Microsoft.EntityFrameworkCore;
using Xacte.Patient.Data.Contexts;
using Xacte.Patient.Data.Repositories.Interfaces;

namespace Xacte.Patient.Data.Repositories
{
    public sealed class PatientRepository : IPatientRepository
    {
        private readonly PatientDbContext _context;

        public PatientRepository(PatientDbContext context)
        {
            _context = context;
        }

        public Task CreateAsync(Entities.Patient entity)
        {
            _context.Patients.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Guid guid)
        {
            return _context.Patients.Where(w => w.Guid == guid).ExecuteDeleteAsync();
        }

        public Task<List<Entities.Patient>> GetAsync()
        {
            return _context.Patients.AsNoTracking().ToListAsync();
        }

        public Task<Entities.Patient> GetAsync(Guid guid)
        {
            return _context.Patients.FirstAsync(f => f.Guid == guid);
        }

        public Task<Entities.Patient> GetAsync(int id)
        {
            return _context.Patients.FirstAsync(f => f.Id == id);
        }

        public Task UpdateAsync(Entities.Patient entity)
        {
            return _context.Patients.ExecuteUpdateAsync(p =>
                p.SetProperty(x => x.FirstName, x => "new value"));
        }
    }
}
