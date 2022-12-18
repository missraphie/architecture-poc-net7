namespace Xacte.Patient.Data.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task CreateAsync(Entities.Patient entity);
        Task DeleteAsync(Guid guid);
        Task<Entities.Patient> GetAsync(Guid guid);
        Task<Entities.Patient> GetAsync(int id);
        Task<List<Entities.Patient>> GetAsync();
        Task UpdateAsync(Entities.Patient entity);
    }
}