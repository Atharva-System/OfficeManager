﻿using Microsoft.SqlServer.Server;
using System.Data;

namespace OfficeManager.Application.Dtos
{
    //BI stands for Bulk Insertion in BIEmployeeDto
    public class BulkImportEmployeeDTO
    {
        public int Id { get; set; }
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;

        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsValid { get; set; } = true;
        public string PasswordHash { get; set; } = string.Empty;
        public int RoleId { get; set; } = 0;
        public List<string> ValidationErros { get; set; } = new List<string>();
    }

    public class BulkImportEmployeeValidInvalidListDTO
    {
        public List<BulkImportEmployeeDTO> NewEmployeeList { get; set; } = new List<BulkImportEmployeeDTO>();
        public List<BulkImportEmployeeDTO> UpdateEmployeeList { get; set; } = new List<BulkImportEmployeeDTO>();
        public List<BulkImportEmployeeDTO> InvalidImportEmployeeList { get; set; } = new List<BulkImportEmployeeDTO>();
    }

    public class BulkImportEmployee
    {
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public int RoleId { get; set; } = 0;

        public static IEnumerable<SqlDataRecord> ToSqlDataRecord(List<BulkImportEmployee> employees)
        {
            var employeeNo = new SqlMetaData("EmployeeNo", SqlDbType.Int);
            var employeeName = new SqlMetaData("EmployeeName", SqlDbType.NVarChar, 100);
            var departmentId = new SqlMetaData("DepartmentId", SqlDbType.Int);
            var designationId = new SqlMetaData("DesignationId", SqlDbType.Int);
            var dateOfJoining = new SqlMetaData("DateOfJoining", SqlDbType.DateTime);
            var dateOfBirth = new SqlMetaData("DateOfBirth", SqlDbType.DateTime);
            var email = new SqlMetaData("Email", SqlDbType.NVarChar, 100);
            var roleId = new SqlMetaData("RoleId", SqlDbType.Int);
            var passwordHash = new SqlMetaData("PasswordHash", SqlDbType.NVarChar, 100);
            var record = new SqlDataRecord(employeeNo, employeeName, departmentId, designationId, email, dateOfJoining, dateOfBirth, roleId, passwordHash);

            foreach (var item in employees)
            {
                record.SetInt32(0, item.EmployeeNo);
                record.SetString(1, item.EmployeeName);
                record.SetInt32(2, item.DepartmentId);
                record.SetInt32(3, item.DesignationId);
                record.SetString(4, "");
                record.SetDateTime(5, item.DateOfJoining);
                record.SetDateTime(6, item.DateOfBirth);
                record.SetInt32(7, item.RoleId);
                record.SetString(8, "");
                yield return record;
            }
        }
    }
}