using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeManager.Domain.Entities
{
    public class EmployeeSkill : BaseAuditableEntity
    {
        public int EmployeeId { get; set; }
        public int skillId { get; set; }
        public int levelId { get; set; }
        public int rateId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        [ForeignKey("skillId")]
        public Skill Skill { get; set; }
        [ForeignKey("levelId")]
        public SkillLevel SkillLevel { get; set; }
        [ForeignKey("rateId")]
        public SkillRate SkillRate { get; set; }

    }
}
