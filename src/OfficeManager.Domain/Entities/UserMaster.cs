using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeManager.Domain.Entities
{
    [Table("User")]
    public class UserMaster : BaseAuditableEntity
    {
        public int EmployeeID { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }
    }
}
