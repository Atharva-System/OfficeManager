using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManager.Application.Common.Models
{
    public class EmployeeDto
    {
        public int EmployeeNo { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmployeeName { get; set; }
    }
}
