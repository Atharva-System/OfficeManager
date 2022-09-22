using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.Feature.Departments.Queries
{
    public record SearchDepartments : IRequest<IResponse>
    {
        public string search { get; init; } = string.Empty;
        public int Page_No { get; set; } = 1;
        public int Page_Size { get; set; } = 10;
        public string SortingColumn { get; set; } = "Name";
        public string SortingDirection { get; set; } = "ASC";
    }
    public class SearchDepartmentHandler : IRequestHandler<SearchDepartments, IResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SearchDepartmentHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IResponse> Handle(SearchDepartments request, CancellationToken cancellationToken)
        {
            PaginatedList<DepartmentDTO> departments = new PaginatedList<DepartmentDTO>(new List<DepartmentDTO>(),0,request.Page_No, request.Page_Size);
            var query = _context.Department.AsQueryable().OrderBy(request.SortingColumn, (request.SortingDirection.ToLower() == "desc" ? false : true));
            if (string.IsNullOrEmpty(request.search))
            {
                departments = await query
                    .ProjectTo<DepartmentDTO>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync<DepartmentDTO>(request.Page_No, request.Page_Size);
            }
            else
            {
                departments = await _context.Department
                    .AsNoTracking()
                    .Where(d => d.Name.Contains(request.search))
                    .ProjectTo<DepartmentDTO>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync<DepartmentDTO>(request.Page_No, request.Page_Size);
            }
            
            if(departments.Items.Count == 0)
            {
                return new ErrorResponse(StatusCodes.BadRequest,Messages.NoDataFound);
            }

            return new DataResponse<PaginatedList<DepartmentDTO>>(departments,StatusCodes.Accepted,Messages.DataFound);
        }
    }
}
