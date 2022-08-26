using AutoMapper;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManager.Application.Employees.Queries.GetAllEmployees
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class EmployeeListResponse
    {
        public EmployeeListResponse()
        {
            Employees = new List<EmployeeDto>();
            TotalCount = 0;
            TotalPages = 0;
            PageNumber = 1;
            PageSize = 10;
        }
        public List<EmployeeDto> Employees { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages {get;set;}
    }
}
