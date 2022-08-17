using Dapper;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using System.Data;
using System.Data.SqlClient;

namespace OfficeManager.Application.Employees.Queries.GetAllEmployees
{
    public record GetAllEmployeesQuery(
        string Search,
        int DepartmentId,
        int DesignationId,
        int? RoleId,
        DateTime? DateOfBirthFrom,
        DateTime? DateOfBirthTo,
        DateTime? DateOfJoiningFrom,
        DateTime? DateOfJoiningTo,
        int PageNo,
        int PageSize
    ) : IRequest<Response<EmployeeListResponse>>;

    public class GetAllEmployeeQueryHandler : IRequestHandler<GetAllEmployeesQuery, Response<EmployeeListResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllEmployeeQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<EmployeeListResponse>> Handle(GetAllEmployeesQuery request,CancellationToken cancellationToken)
        {
            Response<EmployeeListResponse> response = new Response<EmployeeListResponse>();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Name", request.Search);
                parameters.Add("@DepartmentId", request.DepartmentId);
                parameters.Add("@DesignationId", request.DesignationId);
                parameters.Add("@DOBFromDate", request.DateOfBirthFrom == null ? Convert.ToDateTime("1753-01-02") : request.DateOfBirthFrom.Value);
                parameters.Add("@DOBToDate", request.DateOfBirthTo == null ? Convert.ToDateTime("9999-12-30") : request.DateOfBirthTo.Value);
                parameters.Add("@DOJFromDate", request.DateOfJoiningFrom == null ? Convert.ToDateTime("1753-01-02") : request.DateOfJoiningFrom.Value);
                parameters.Add("@@DOJToDate", request.DateOfJoiningTo == null ? Convert.ToDateTime("9999-12-30") : request.DateOfJoiningTo.Value);
                //parameters.Add("@PageNumber", request.PageNo);
                //parameters.Add("@PageSize", request.PageSize);
                using (var connection = new SqlConnection(_context.GetConnectionString))
                {
                    response._Data = new EmployeeListResponse();
                    response._Data.Employees = new List<EmployeeDto>();
                    response._Data.Employees = (await connection.QueryAsync<EmployeeDto>("dbo.SearchEmployees", parameters
                        , commandType: CommandType.StoredProcedure)).ToList();
                    response._Data.TotalPages = (response._Data.Employees.Count / request.PageSize);
                    response._Data.TotalCount = response._Data.Employees.Count;

                    if (response._Data.TotalPages * request.PageSize < response._Data.TotalCount)
                    {
                        response._Data.TotalPages += 1;
                    }
                    response._Data.Employees = response._Data.Employees.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response._Data.PageNumber = request.PageNo;
                    response._Data.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
                    if (response._Data.Employees.Count > 0 && response._Data.TotalCount > 0)
                    {
                        response._IsSuccess = true;
                        response._StatusCode = "200";
                        response._Message = "All the data found";
                    }
                    else
                    {
                        response._Message = "No records found";
                        response._IsSuccess = false;
                        response._StatusCode = "404";
                    }
                }
                return response;
            }
            catch(Exception ex)
            {
                response._Errors.Add("Data or connection issue, please check internet or contact administrator.");
                response._IsSuccess = false;
                response._StatusCode = "500";
                return response;
            }

        }
    }
}
