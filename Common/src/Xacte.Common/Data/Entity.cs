namespace Xacte.Common.Data
{
    public abstract class Entity : EntityIdentifier
    {
        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public string ModifiedByName { get; set; } = string.Empty;
        public DateTime ModifiedOn { get; set; }
    }
}
