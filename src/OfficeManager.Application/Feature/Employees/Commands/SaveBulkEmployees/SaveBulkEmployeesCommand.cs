using Dapper;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using System.Data;
using System.Data.SqlClient;

namespace OfficeManager.Application.Employees.Commands.SaveBulkEmployees
{
    public record SaveBulkEmployeesCommand : IRequest<Response<object>>
    {
        public List<BulkImportEmployeeDTO> employees { get; set; } = new List<BulkImportEmployeeDTO>();
    }

    public class SaveBulkEmployeesCommandHandler : IRequestHandler<SaveBulkEmployeesCommand, Response<object>>
    {
        private readonly IApplicationDbContext _context;
        public SaveBulkEmployeesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<object>> Handle(SaveBulkEmployeesCommand request, CancellationToken cancellationToken)
        {
            Response<object> response = new Response<object>();
            try
            {
                request.employees.ForEach(emp =>
                {
                    emp.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Atharva@123");
                });
                using (SqlConnection con = new SqlConnection(_context.GetConnectionString))
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
                    parameters.Add("@IsSuccess", false, direction:System.Data.ParameterDirection.InputOutput);
                    con.Execute("AddBulkEmployees", parameters,commandType:System.Data.CommandType.StoredProcedure);
                    if (parameters.Get<bool>("@IsSuccess"))
                    {
                        response._Message = "All the employees are inserted successfully";
                        response._StatusCode = "200";
                        response._IsSuccess = true;
                    }
                }
                    //request.employees.ForEach(emp =>
                    //{
                    //    _context.BeginTransaction();
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
                    //    _context.Employees.Add(employee);

                    //    _context.SaveChangesAsync(cancellationToken);

                    //    UserMaster user = new UserMaster
                    //    {
                    //        EmployeeID = employee.Id,
                    //        Email = emp.Email,
                    //        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Atharva@123")
                    //    };
                    //    _context.Users.Add(user);

                    //    _context.SaveChangesAsync(cancellationToken);

                    //    UserRoleMapping userRole = new UserRoleMapping()
                    //    {
                    //        UserId = user.Id,
                    //        RoleId = emp.RoleId
                    //    };
                    //    _context.UserRoleMapping.Add(userRole);
                    //    _context.SaveChangesAsync(cancellationToken);

                    //    _context.CommitTransaction();
                    //});
                    
                return response;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response._Message = "There is some error in data";
                response._Errors.Add(ex.Message);
                response._IsSuccess = false;
                response._StatusCode = "500";
                return response;
            }
        }
    }
}
