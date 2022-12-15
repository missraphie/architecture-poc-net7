namespace Xacte.Common.Data
{
    public abstract class EntityConfiguration<TBase> : EntityIdentifierConfiguration<TBase> where TBase : Entity
    {
        protected EntityConfiguration(bool autoGenerateUniqueIdentifier = true)
            : base(autoGenerateUniqueIdentifier)
        {
        }
    }
}
