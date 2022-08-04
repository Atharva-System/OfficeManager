
using System.ComponentModel.DataAnnotations;

namespace OfficeManager.Domain.Entities
{
    public class Employee : BaseAuditableEntity
    {
        public int EmployeeNo { get; set; }
        public int DesignationId { get; set; } = 0;
        public int DepartmentId { get; set; } = 0;
        [StringLength(500)]
        public string EmployeeName { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
    }
}
