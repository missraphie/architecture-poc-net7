using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xacte.Common.Data.Configurations;

namespace Xacte.Patient.Data.Configurations
{
    internal sealed class PatientConfiguration : EntityConfiguration<Entities.Patient>
    {
        public PatientConfiguration()
            : base(autoGenerateUniqueIdentifier: true)
        {
        }

        public override void Configure(EntityTypeBuilder<Entities.Patient> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
