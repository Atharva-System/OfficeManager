using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeManager.Domain.Entities
{
    public class ProfileMaster : BaseAuditableEntity
    {
        public string UserId { get; set; }
        public string Contact { get; set; }
        public string PersonalEmail { get; set; }
        public string? ProfilePic { get; set; } = "";
        public Nullable<DateTime> DateOfBirth { get; set; }
        public DateTime? DateOfJoining { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
