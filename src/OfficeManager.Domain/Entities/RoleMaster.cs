
using System.ComponentModel.DataAnnotations;

namespace OfficeManager.Domain.Entities
{
    public class RoleMaster
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
