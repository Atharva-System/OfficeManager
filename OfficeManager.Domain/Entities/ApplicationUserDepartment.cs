namespace OfficeManager.Domain.Entities
{
    public class ApplicationUserDepartment : BaseAuditableEntity
    {
        public Guid DepartmentId { get; set; }
        public string UserId { get; set; }
    }
}
