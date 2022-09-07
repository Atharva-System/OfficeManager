using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using System.Data;
using System.Data.SqlClient;

namespace OfficeManager.Application.Feature.Employees.Queries
{
    public record GetAllEmployees: IRequest<Response<PaginatedList<EmployeeDTO>>>
    {

        public string Search { get; init; }
        public int DepartmentId { get; init; }
        public int DesignationId { get; init; }
        public int? RoleId { get; init; }
        public DateTime? DateOfBirthFrom { get; init; }
        public DateTime? DateOfBirthTo { get; init; }
        public DateTime? DateOfJoiningFrom { get; init; }
        public DateTime? DateOfJoiningTo { get; init; }
        public int Page_No { get; init; } = 1;
        public int Page_Size { get; init; } = 10;
    }

    public class GetAllEmployeeQueryHandler : IRequestHandler<GetAllEmployees, Response<PaginatedList<EmployeeDTO>>>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper _mapper;

        public GetAllEmployeeQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            Context = context;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedList<EmployeeDTO>>> Handle(GetAllEmployees request, CancellationToken cancellationToken)
        {
            Response<PaginatedList<EmployeeDTO>> response = new Response<PaginatedList<EmployeeDTO>>();
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
                using (var connection = new SqlConnection(Context.GetConnectionString))
                {
                    response.Data = new PaginatedList<EmployeeDTO>(new List<EmployeeDTO>(), 0, request.Page_No, request.Page_Size);
                    response.Data = await (await connection.QueryAsync<EmployeeDTO>("dbo.SearchEmployees", parameters
                        , commandType: CommandType.StoredProcedure))
                        .ToList()
                        .PaginatedListAsync<EmployeeDTO>(request.Page_No, request.Page_Size);

                    if(response.Data.Items.Count > 0)
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