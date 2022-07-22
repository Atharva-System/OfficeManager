namespace OfficeManager.Domain.Entities
{
    public class UserRole : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual List<UserMaster> Users { get; set; }
    }
}
