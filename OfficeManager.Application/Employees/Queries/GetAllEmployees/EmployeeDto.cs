using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManager.Application.Employees.Queries.GetAllEmployees
{
    public class EmployeeDto
    {
        public string id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
    }
}
