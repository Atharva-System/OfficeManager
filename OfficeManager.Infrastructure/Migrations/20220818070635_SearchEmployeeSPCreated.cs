using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeManager.Infrastructure.Migrations
{
    public partial class SearchEmployeeSPCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			string createProcSql = @"
                CREATE OR ALTER   PROCEDURE [dbo].[SearchEmployees]
					@Name nvarchar(200) = NULL,
					@DepartmentId int = NULL,
					@DesignationId int = NULL,
					@DOBFromDate datetime = NULL,
					@DOBToDate datetime = NULL,
					@DOJFromDate datetime = NULL,
					@DOJToDate datetime = NULL
				AS
				BEGIN
					SET NOCOUNT ON
					SET @Name  = '%' + @Name + '%'
					SELECT
						emp.Id AS EmployeeId
						,emp.EmployeeNo
						,emp.EmployeeName
						,emp.Email
						,R.Name AS EmployeeRole
						,Dep.Name AS Department
						,Des.Name AS Designation
						,emp.DateOfBirth
						,emp.DateOfJoining
					FROM
						Employees emp
						LEFT JOIN [User] U
							ON emp.Id = U.EmployeeID
						LEFT JOIN UserRoleMapping URM
							ON U.Id = URM.UserId
						LEFT JOIN Roles R
							ON URM.RoleId = R.Id
						LEFT JOIN DepartMent Dep
							ON emp.DepartmentId = Dep.Id
						LEFT JOIN Designation Des
							ON emp.DesignationId = Des.Id
					WHERE
						(@Name IS NULL 
						OR (emp.EmployeeName like @Name))
						AND (@DepartmentId IS NULL OR @DepartmentId = 0
							OR (emp.DepartmentId = @DepartmentId))
						AND (@DesignationId IS NULL OR @DesignationId = 0
							OR (emp.DesignationId = @DesignationId))
						AND (@DOBFromDate IS NULL
							OR (emp.DateOfBirth >= @DOBFromDate))
						AND (@DOBToDate IS NULL OR emp.DateOfBirth <= @DOBToDate)
						AND (@DOJFromDate IS NULL OR emp.DateOfJoining >= @DOJFromDate)
						AND (@DOJToDate IS NULL OR emp.DateOfJoining <= @DOJToDate)
					ORDER BY
						emp.EmployeeNo
				END";

			migrationBuilder.Sql(createProcSql);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
