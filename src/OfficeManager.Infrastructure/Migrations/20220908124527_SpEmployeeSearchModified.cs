using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeManager.Infrastructure.Migrations
{
    public partial class SpEmployeeSearchModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			string alterStoredProcedure = @"
                 CREATE OR ALTER     PROCEDURE [dbo].[SearchEmployees]
					@Search nvarchar(200) = NULL,
					@DepartmentId int = NULL,
					@DesignationId int = NULL,
					@DOBFromDate datetime = NULL,
					@DOBToDate datetime = NULL,
					@DOJFromDate datetime = NULL,
					@DOJToDate datetime = NULL
				AS
				BEGIN
					SET NOCOUNT ON
					SET @Search  = '%' + @Search + '%'
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
						LEFT JOIN Department Dep
							ON emp.DepartmentId = Dep.Id
						LEFT JOIN Designation Des
							ON emp.DesignationId = Des.Id
					WHERE
						(@Search IS NULL 
						OR (emp.EmployeeName like @Search OR emp.Email like @Search))
						AND (@DepartmentId IS NULL OR @DepartmentId = 0
							OR (emp.DepartmentId = @DepartmentId))
						AND (@DesignationId IS NULL OR @DesignationId = 0
							OR (emp.DesignationId = @DesignationId))
						AND (@DOBFromDate IS NULL
							OR (emp.DateOfBirth >= @DOBFromDate))
						AND (@DOBToDate IS NULL OR emp.DateOfBirth <= @DOBToDate)
						AND (@DOJFromDate IS NULL OR emp.DateOfJoining >= @DOJFromDate)
						AND (@DOJToDate IS NULL OR emp.DateOfJoining <= @DOJToDate)
				END
                ";

			migrationBuilder.Sql(alterStoredProcedure);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
