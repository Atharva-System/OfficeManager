using System.ComponentModel.DataAnnotations;

namespace OfficeManager.Domain.Entities
{
    public class Department : BaseAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public virtual List<Employee> Employees { get; set; }
    }
}