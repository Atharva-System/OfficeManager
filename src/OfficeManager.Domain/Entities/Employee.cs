
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeManager.Domain.Entities
{
    public class Employee : BaseAuditableEntity
    {
        public int EmployeeNo { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        [StringLength(500)]
        public string EmployeeName { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        [ForeignKey("DesignationId")]
        public Designation Designation { get; set; }
    }
}
