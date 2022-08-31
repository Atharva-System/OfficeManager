using Dapper;
using MediatR;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using System.Data;
using System.Data.SqlClient;

namespace OfficeManager.Application.Feature.Employees.Queries
{
    public record GetAllEmployees(
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

    public class GetAllEmployeeQueryHandler : IRequestHandler<GetAllEmployees, Response<EmployeeListResponse>>
    {
        private readonly IApplicationDbContext context;

        public GetAllEmployeeQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Response<EmployeeListResponse>> Handle(GetAllEmployees request, CancellationToken cancellationToken)
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
                using (var connection = new SqlConnection(context.GetConnectionString))
                {
                    response.Data = new EmployeeListResponse();
                    response.Data.Employees = new List<EmployeeDTO>();
                    response.Data.Employees = (await connection.QueryAsync<EmployeeDTO>("dbo.SearchEmployees", parameters
                        , commandType: CommandType.StoredProcedure)).ToList();
                    response.Data.TotalPages = response.Data.Employees.Count / request.PageSize;
                    response.Data.TotalCount = response.Data.Employees.Count;

                    if (response.Data.TotalPages * request.PageSize < response.Data.TotalCount)
                    {
                        response.Data.TotalPages += 1;
                    }
                    response.Data.Employees = response.Data.Employees.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.Data.PageNumber = request.PageNo;
                    response.Data.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
                    if (response.Data.Employees.Count > 0 && response.Data.TotalCount > 0)
                    {
                        response.IsSuccess = true;
                        response.StatusCode = StausCodes.Accepted;
                        response.Message = Messages.DataFound;
                    }
                    else
                    {
                        response.Message = Messages.NoDataFound;
                        response.IsSuccess = false;
                        response.StatusCode = StausCodes.NotFound;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Errors.Add(Messages.IssueWithData);
                response.IsSuccess = false;
                response.StatusCode = StausCodes.InternalServerError;
                return response;
            }

        }
    }
}
