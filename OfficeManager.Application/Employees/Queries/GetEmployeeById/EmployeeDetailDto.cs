using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Employees.Queries.GetEmployeeById
{
    public class EmployeeDetailDto
    {
        public int UserId { get; set; }
        public int EmployeeNo { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public int RoleId { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public List<EmployeeSkill> skills { get; set; }
    }
}
