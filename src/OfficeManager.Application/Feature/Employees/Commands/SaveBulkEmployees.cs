using Dapper;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using System.Data;
using System.Data.SqlClient;

namespace OfficeManager.Application.Feature.Employees.Commands
{
    public record SaveBulkEmployees : IRequest<IResponse>
    {
        public List<BulkImportEmployeeDTO> employees { get; set; } = new List<BulkImportEmployeeDTO>();
    }

    public class SaveBulkEmployeesCommandHandler : IRequestHandler<SaveBulkEmployees, IResponse>
    {
        private readonly IApplicationDbContext Context;

        public SaveBulkEmployeesCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<IResponse> Handle(SaveBulkEmployees request, CancellationToken cancellationToken)
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
                    return new SuccessResponse(StatusCodes.Accepted, "All employeess added successfully.");
                }
            }

            return new ErrorResponse(StatusCodes.BadRequest,"Data has issue, please check the data.");
        }
    }
}