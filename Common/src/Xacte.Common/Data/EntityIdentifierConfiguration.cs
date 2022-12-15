using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Xacte.Common.Data
{
    public abstract class EntityIdentifierConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : EntityIdentifier
    {
        /// <summary>
        /// Controls if guid value should be generated from database or manually.
        /// </summary>
        private readonly bool _autoGenerateUniqueIdentifier;

        protected EntityIdentifierConfiguration(bool autoGenerateUniqueIdentifier = true)
        {
            _autoGenerateUniqueIdentifier = autoGenerateUniqueIdentifier;
        }

        public virtual void Configure(EntityTypeBuilder<TBase> builder)
        {
            if (_autoGenerateUniqueIdentifier)
            {
                // Note (rthouin): This will only apply to the TBase entity, not the child.
                // If you need to add Guid auto-generation to child entities, you must do it in you code.
                builder.Property(p => p.Guid).HasDefaultValueSql("newid()").ValueGeneratedOnAdd();
            }

            builder.HasIndex(p => p.Guid).IsUnique();
        }
    }
}
