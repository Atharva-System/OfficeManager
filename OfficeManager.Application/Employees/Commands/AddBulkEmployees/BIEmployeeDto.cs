using Microsoft.SqlServer.Server;
using System.Data;

namespace OfficeManager.Application.Employees.Commands.AddBulkEmployees
{
    //BI stands for Bulk Insertion in BIEmployeeDto
    public class BIEmployeeDto
    {
        public int Id { get; set; }
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }

        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string Email { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsValid { get; set; } = true;
        public string PasswordHash { get; set; }
        public int RoleId { get; set; } = 0;
        public List<string> ValidationErros { get; set; } = new List<string>();
    }

    public class BIEmployee
    {
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; } = 0;

        public static IEnumerable<SqlDataRecord> ToSqlDataRecord(List<BIEmployee> employees)
        {
            var employeeNo = new SqlMetaData("EmployeeNo", SqlDbType.Int);
            var employeeName = new SqlMetaData("EmployeeName", SqlDbType.NVarChar,100);
            var departmentId = new SqlMetaData("DepartmentId", SqlDbType.Int);
            var designationId = new SqlMetaData("DesignationId", SqlDbType.Int);
            var dateOfJoining = new SqlMetaData("DateOfJoining", SqlDbType.DateTime);
            var dateOfBirth = new SqlMetaData("DateOfBirth", SqlDbType.DateTime);
            var email = new SqlMetaData("Email", SqlDbType.NVarChar, 100);
            var roleId = new SqlMetaData("RoleId", SqlDbType.Int);
            var passwordHash = new SqlMetaData("PasswordHash", SqlDbType.NVarChar, 100);
            var record = new SqlDataRecord(employeeNo, employeeName, departmentId, designationId,
                dateOfBirth, dateOfJoining, email, roleId, passwordHash);
            foreach(var item in employees)
            {
                record.SetInt32(0, item.EmployeeNo);
                record.SetString(1, item.EmployeeName);
                record.SetInt32(2, item.DepartmentId);
                record.SetInt32(3, item.DesignationId);
                record.SetDateTime(4, item.DateOfBirth);
                record.SetDateTime(5, item.DateOfJoining);
                record.SetString(6, "");
                record.SetInt32(7, item.RoleId);
                record.SetString(8, item.PasswordHash);
                yield return record;
            }
        }
    }
}
