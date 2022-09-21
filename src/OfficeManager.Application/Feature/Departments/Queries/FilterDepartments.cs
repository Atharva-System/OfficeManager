using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Departments.Queries
{
    public record FilterDepartments : IRequest<IResponse>
    {
        public string filterString { get; init; } = string.Empty;
        public int Page_No { get; set; } = 1;
        public int Page_Size { get; set; } = 10;
        public string SortingColumn { get; set; } = "Name";
        public string SortingDirection { get; set; } = "ASC";
    }

    public class FilterDepartmentHandler : IRequestHandler<FilterDepartments, IResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFilterLinq _filterLinq;

        public FilterDepartmentHandler(IApplicationDbContext context, IMapper mapper, IFilterLinq filterLinq)
        {
            _context = context;
            _mapper = mapper;
            _filterLinq = filterLinq;
        }

        public async Task<IResponse> Handle(FilterDepartments request, CancellationToken cancellationToken)
        {
            PaginatedList<DepartmentDTO> departments = new PaginatedList<DepartmentDTO>(new List<DepartmentDTO>(), 0, request.Page_No, request.Page_Size);
            IQueryable<Department> query;


            if (String.IsNullOrEmpty(request.filterString) || String.IsNullOrWhiteSpace(request.filterString))
            {
                query = _context.Department.AsQueryable().OrderBy(request.SortingColumn, (request.SortingDirection.ToLower() == "desc" ? false : true));
                return new DataResponse<PaginatedList<DepartmentDTO>>(await query
                    .ProjectTo<DepartmentDTO>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync<DepartmentDTO>(request.Page_No, request.Page_Size)
                    ,StatusCodes.Accepted
                    ,Messages.DataFound);
            }
            string[] filters = request.filterString.Split(',');

            query = _context.Department;

            Dictionary<string, string> filterValues = new Dictionary<string, string>();
            foreach (string filter in filters)
            {
                if (filter.Contains("="))
                    filterValues.Add(filter.Split('=')[0].Trim(), filter.Split('=')[1].Trim());
            }

            var filterExpression = _filterLinq.GetWherePredicate<Department>(filterValues);

            return new DataResponse<PaginatedList<DepartmentDTO>>(await query
                .Where(filterExpression)
                .ProjectTo<DepartmentDTO>(_mapper.ConfigurationProvider)
                .OrderBy(request.SortingColumn, (request.SortingDirection.ToLower() == "desc" ? false : true))
                .PaginatedListAsync<DepartmentDTO>(request.Page_No, request.Page_Size)
                , StatusCodes.Accepted
                , Messages.DataFound);
        }
    }
}