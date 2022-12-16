using Xacte.Common.Data.Entities;

namespace Xacte.Common.Data.Configurations
{
    public abstract class EntityConfiguration<TBase> : EntityIdentifierConfiguration<TBase> where TBase : Entity
    {
        protected EntityConfiguration(bool autoGenerateUniqueIdentifier = true)
            : base(autoGenerateUniqueIdentifier)
        {
        }
    }
}
