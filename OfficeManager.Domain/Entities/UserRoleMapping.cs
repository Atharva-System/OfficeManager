using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeManager.Domain.Entities
{
    public class UserRoleMapping : BaseAuditableEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("UserId")]
        public UserMaster Users { get; set; }
        [ForeignKey("RoleId")]
        public RoleMaster Roles { get; set; }
    }
}
