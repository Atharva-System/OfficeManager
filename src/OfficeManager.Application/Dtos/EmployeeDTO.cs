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

    public class EmployeeListResponse
    {
        public EmployeeListResponse()
        {
            Employees = new List<EmployeeDTO>();
            TotalCount = 0;
            TotalPages = 0;
            PageNumber = 1;
            PageSize = 10;
        }
        public List<EmployeeDTO> Employees { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
