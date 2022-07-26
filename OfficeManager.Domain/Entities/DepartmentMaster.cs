namespace OfficeManager.Domain.Entities
{
    public class DepartmentMaster : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
