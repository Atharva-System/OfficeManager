using OfficeManager.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeManager.Domain.Entities
{
    public class UserMaster : BaseAuditableEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
        public virtual UserProfile Profile { get; set; }
        [ForeignKey("RoleId")]
        public UserRole Role { get; set; }
    }
}
