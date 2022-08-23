namespace OfficeManager.Domain.Entities
{
    public class Skill : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}