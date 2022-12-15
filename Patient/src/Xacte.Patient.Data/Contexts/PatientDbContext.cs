using Microsoft.EntityFrameworkCore;
using Xacte.Common.Data;
using Xacte.Patient.Data.Configurations;

namespace Xacte.Patient.Data.Contexts
{
    public sealed class PatientDbContext : XacteDbContext
    {
        public PatientDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Entities.Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.ApplyConfiguration(new PatientConfiguration());
        }
    }
}
