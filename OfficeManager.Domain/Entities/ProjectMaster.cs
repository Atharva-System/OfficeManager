namespace OfficeManager.Domain.Entities
{
    public class ProjectMaster : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
