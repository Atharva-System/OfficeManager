using OfficeManager.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeManager.Domain.Entities
{
    public class UserProfile : BaseAuditableEntity
    {
        public Guid UserId { get; set; }
        public string Contact { get; set; }
        public string PersonalEmail { get; set; }
        public DateTime? DateOfJoining { get; set; }
        [ForeignKey("UserId")]
        public UserMaster User { get; set; }
    }
}
