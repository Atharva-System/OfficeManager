using Dapper;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using System.Data;
using System.Data.SqlClient;

namespace OfficeManager.Application.Feature.Employees.Commands
{
    public record SaveBulkEmployees : IRequest<Response<object>>
    {
        public List<BulkImportEmployeeDTO> employees { get; set; } = new List<BulkImportEmployeeDTO>();
    }

    public class SaveBulkEmployeesCommandHandler : IRequestHandler<SaveBulkEmployees, Response<object>>
    {
        private readonly IApplicationDbContext Context;

        public SaveBulkEmployeesCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<Response<object>> Handle(SaveBulkEmployees request, CancellationToken cancellationToken)
        {
            Response<object> response = new Response<object>();
            try
            {
                request.employees.ForEach(emp =>
                {
                    emp.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Atharva@123");
                });
                using (SqlConnection con = new SqlConnection(Context.GetConnectionString))
                {
                    var lstEmployees = request.employees.Select(emp => new BulkImportEmployee
                    {
                        DepartmentId = emp.DepartmentId,
                        DesignationId = emp.DesignationId,
                        EmployeeNo = emp.EmployeeNo,
                        EmployeeName = emp.EmployeeName,
                        PasswordHash = emp.PasswordHash,
                        RoleId = emp.RoleId,
                        DateOfBirth = emp.DateOfBirth,
                        DateOfJoining = emp.DateOfJoining
                    });
                    var parameters = new DynamicParameters();
                    //parameters.Add("@employees", lstEmployees);
                    parameters.Add("@employees", BulkImportEmployee.ToSqlDataRecord(lstEmployees.ToList()).AsTableValuedParameter("UT_Employee"));
                    parameters.Add("@IsSuccess", false, direction: ParameterDirection.InputOutput);
                    con.Execute("AddBulkEmployees", parameters, commandType: CommandType.StoredProcedure);
                    if (parameters.Get<bool>("@IsSuccess"))
                    {
                        response.Message = "All the employees are inserted successfully";
                        response.StatusCode = StausCodes.Accepted;
                        response.IsSuccess = true;
                    }
                }
                //request.employees.ForEach(emp =>
                //{
                //    Context.BeginTransaction();
                //    Employee employee = new Employee
                //    {
                //        EmployeeNo = emp.EmployeeNo,
                //        DesignationId = emp.DesignationId,
                //        DepartmentId = emp.DepartmentId,
                //        DateOfJoining = emp.DateOfJoining,
                //        DateOfBirth = emp.DateOfBirth,
                //        Email = emp.Email,
                //        EmployeeName = emp.EmployeeName
                //    };
                //    Context.Employees.Add(employee);

                //    Context.SaveChangesAsync(cancellationToken);

                //    UserMaster user = new UserMaster
                //    {
                //        EmployeeID = employee.Id,
                //        Email = emp.Email,
                //        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Atharva@123")
                //    };
                //    Context.Users.Add(user);

                //    Context.SaveChangesAsync(cancellationToken);

                //    UserRoleMapping userRole = new UserRoleMapping()
                //    {
                //        UserId = user.Id,
                //        RoleId = emp.RoleId
                //    };
                //    Context.UserRoleMapping.Add(userRole);
                //    Context.SaveChangesAsync(cancellationToken);

                //    Context.CommitTransaction();
                //});

                return response;
            }
            catch (Exception ex)
            {
                response.Data = string.Empty;
                response.Message = Messages.IssueWithData;
                response.Errors.Add(ex.Message);
                response.IsSuccess = false;
                response.StatusCode = StausCodes.InternalServerError;
                return response;
            }
        }
    }
}