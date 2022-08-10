using Microsoft.EntityFrameworkCore.Migrations;

namespace OfficeManager.Infrastructure.Migrations
{
    public partial class StoredProcedureCustomTypeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			string createTypeSql = @"
				EXEC (CREATE TYPE [dbo].[UT_Employee] AS TABLE(
					[EmployeeNo] [int] NULL,
					[EmployeeName] [nvarchar](max) NULL,
					[DepartmentId] [int] NULL,
					[DesignationId] [int] NULL,
					[Email] [nvarchar](max) NULL,
					[DateOfJoining] [datetime] NULL,
					[DateOfBirth] [datetime] NULL,
					[RoleId] [int] NULL,
					[PasswordHash] [nvarchar](max) NULL
				))";
			migrationBuilder.Sql(createTypeSql);


			string createProcSql = @"EXEC (
                CREATE OR ALTER PROCEDURE AddBulkEmployees
					@employees UT_Employee READONLY,
					@IsSuccess bit = 0 OUTPUT
				AS
				BEGIN

					DECLARE @Count AS INT = 0
					SET NOCOUNT ON
					BEGIN TRY
					BEGIN TRANSACTION

					SELECT @Count = COUNT(*) FROM @employees

					UPDATE emp2
						SET emp2.EmployeeNo = emp.EmployeeNo,
						emp2.EmployeeName = emp.EmployeeName,
						emp2.DepartmentId = emp.DepartmentId,
						emp2.DesignationId = emp.DesignationId,
						emp2.DateOfBirth  =emp.DateOfBirth,
						emp2.DateOfJoining = emp.DateOfJoining,
						emp2.ModifiedBy = 1,
						emp2.ModifiedDate = GETDATE()
					FROM
						@employees emp
						INNER JOIN Employees emp2
						ON emp.EmployeeNo = emp2.EmployeeNo
	 
					INSERT INTO
						Employees
							(EmployeeNo,EmployeeName,Email,DateOfBirth,DateOfJoining,DepartmentId,DesignationId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,IsActive)
						(SELECT
							EmployeeNo,
							EmployeeName,
							'' AS Email,
							DateOfBirth,
							DateOfJoining,
							DepartmentId,
							DesignationId,
							1 AS CreatedBy,
							GETDATE() AS CreatedDate,
							1 AS ModifiedBy,
							GETDATE() AS ModifiedDate,
							1 AS IsActive
						FROM
							@employees
						WHERE
							EmployeeNo NOT IN (SELECT EmployeeNo FROM Employees))

					INSERT INTO
						[User]
							(EmployeeID,PasswordHash,Email,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,IsActive)
						(SELECT
							emp2.Id AS EmployeeID,
							emp.PasswordHash AS PasswordHash,
							'' AS Email,
							1 AS CreatedBy,
							GETDATE() AS CreatedDate,
							1 AS ModifiedBy,
							GETDATE() AS ModifiedDate,
							1 AS IsActive
						FROM
							@employees emp
							INNER JOIN Employees emp2
								ON emp.EmployeeNo = emp2.EmployeeNo
						WHERE
							emp2.Id NOT IN (SELECT EmployeeID FROM [User]))

					INSERT INTO UserRoleMapping
						(UserId,RoleId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,IsActive)
					(SELECT
						U.Id AS UserId,
						emp2.RoleId AS RoleId,
							1 AS CreatedBy,
							GETDATE() AS CreatedDate,
							1 AS ModifiedBy,
							GETDATE() AS ModifiedDate,
							1 AS IsActive
					FROM
						[User] U
						INNER JOIN Employees emp
							ON U.EmployeeID = emp.Id
						INNER JOIN @employees emp2
							ON emp.EmployeeNo = emp2.EmployeeNo
					WHERE
						U.Id NOT IN (SELECT UserId FROM UserRoleMapping WHERE RoleId = 1))

					COMMIT TRANSACTION
					SET @IsSuccess = 1
					END TRY
					BEGIN CATCH
						--RAISEERROR('Execut ' + CAST(@Count AS VARCHAR(10)),1,@IsSuccess)
						SET @IsSuccess = 0

						DECLARE @ErrorMsg NVARCHAR(4000); 
					DECLARE @ErrSeverity INT; 
					DECLARE @ErrState INT; 

					SELECT @ErrorMsg = Error_message(), 
						   @ErrSeverity = Error_severity(), 
						   @ErrState = Error_state(); 

					RAISERROR (@ErrorMsg,
							   @ErrSeverity,
							   @ErrState 
					); 
					END CATCH
				END)";

			migrationBuilder.Sql(createProcSql);

		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
