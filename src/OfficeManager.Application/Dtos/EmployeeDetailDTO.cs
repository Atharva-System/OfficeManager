using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Dtos
{
    public class EmployeeDetailDTO
    {
        public int UserId { get; set; }
        public int EmployeeNo { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;    
        public string Email { get; set; } = string.Empty;
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public int RoleId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public List<EmployeeSkill> skills { get; set; } = new List<EmployeeSkill>();
    }
}
