using System.ComponentModel.DataAnnotations;

namespace OfficeManager.Domain.Entities
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime Created { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public DateTime Modified { get; set; } = DateTime.Now;
        public int ModifiedBy { get; set; }
    }
}
