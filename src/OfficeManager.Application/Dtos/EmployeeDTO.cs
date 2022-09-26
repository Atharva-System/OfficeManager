namespace OfficeManager.Application.Dtos
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string EmployeeRole { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
