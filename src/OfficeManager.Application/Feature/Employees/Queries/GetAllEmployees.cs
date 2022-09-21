using AutoMapper;
using Dapper;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using System.Data;
using System.Data.SqlClient;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.Application.Feature.Employees.Queries
{
    public record GetAllEmployees: IRequest<IResponse>
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
        public string SortingColumn { get; set; } = "EmployeeNo";
        public string SortingDirection { get; set; } = "ASC";
    }

    public class GetAllEmployeeQueryHandler : IRequestHandler<GetAllEmployees, IResponse>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper _mapper;

        public GetAllEmployeeQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            Context = context;
            _mapper = mapper;
        }

        public async Task<IResponse> Handle(GetAllEmployees request, CancellationToken cancellationToken)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Search", request.Search);
            parameters.Add("@DepartmentId", request.DepartmentId);
            parameters.Add("@DesignationId", request.DesignationId);
            parameters.Add("@DOBFromDate", request.DateOfBirthFrom == null ? Convert.ToDateTime("1753-01-02") : request.DateOfBirthFrom.Value);
            parameters.Add("@DOBToDate", request.DateOfBirthTo == null ? Convert.ToDateTime("9999-12-30") : request.DateOfBirthTo.Value);
            parameters.Add("@DOJFromDate", request.DateOfJoiningFrom == null ? Convert.ToDateTime("1753-01-02") : request.DateOfJoiningFrom.Value);
            parameters.Add("@@DOJToDate", request.DateOfJoiningTo == null ? Convert.ToDateTime("9999-12-30") : request.DateOfJoiningTo.Value);
            using (var connection = new SqlConnection(Context.GetConnectionString))
            {
                var employees = await (await connection.QueryAsync<EmployeeDTO>("dbo.SearchEmployees", parameters
                    , commandType: CommandType.StoredProcedure))
                    .AsQueryable()
                    .OrderBy(request.SortingColumn, (request.SortingDirection.ToLower() == "desc" ? false : true))
                    .ToList()
                    .PaginatedListAsync<EmployeeDTO>(request.Page_No, request.Page_Size);
                return new DataResponse<PaginatedList<EmployeeDTO>>(employees,StatusCodes.Accepted);
            }
        }
    }
}