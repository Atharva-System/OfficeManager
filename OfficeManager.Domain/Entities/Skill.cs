using System.ComponentModel.DataAnnotations;

namespace OfficeManager.Domain.Entities
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}