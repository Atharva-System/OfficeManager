using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Departments.Queries
{
    public record FilterDepartments : IRequest<Response<PaginatedList<DepartmentDTO>>>
    {
        public string filterString { get; init; } = string.Empty;
        public int Page_No { get; set; } = 1;
        public int Page_Size { get; set; } = 10;
        public string SortingColumn { get; set; } = "Name";
        public string SortingDirection { get; set; } = "ASC";
    }

    public class FilterDepartmentHandler : IRequestHandler<FilterDepartments, Response<PaginatedList<DepartmentDTO>>>
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

        public async Task<Response<PaginatedList<DepartmentDTO>>> Handle(FilterDepartments request, CancellationToken cancellationToken)
        {
            Response<PaginatedList<DepartmentDTO>> response = new Response<PaginatedList<DepartmentDTO>>();
            try
            {
                PaginatedList<DepartmentDTO> departments = new PaginatedList<DepartmentDTO>(new List<DepartmentDTO>(), 0, request.Page_No, request.Page_Size);
                
                if (String.IsNullOrEmpty(request.filterString) || String.IsNullOrWhiteSpace(request.filterString))
                {
                    var query = _context.Department.AsQueryable().OrderBy(request.SortingColumn, (request.SortingDirection.ToLower() == "desc" ? false : true));
                    departments = await query
                        .ProjectTo<DepartmentDTO>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync<DepartmentDTO>(request.Page_No, request.Page_Size);
                }
                else
                {
                    string filterString = request.filterString.Replace(" ", "");
                    string[] filters = filterString.Split(',');

                    var query = _context.Department;

                    Dictionary<string, string> filterValues = new Dictionary<string, string>();
                    foreach (string filter in filters)
                    {
                        if (filter.Contains("="))
                            filterValues.Add(filter.Split('=')[0].Trim(), filter.Split('=')[1].Trim());
                    }

                    var filterExpression = _filterLinq.GetWherePredicate<Department>(filterValues);

                    departments = await query
                        .Where(filterExpression)
                        .ProjectTo<DepartmentDTO>(_mapper.ConfigurationProvider)
                        .OrderBy(request.SortingColumn, (request.SortingDirection.ToLower() == "desc" ? false : true))
                        .PaginatedListAsync<DepartmentDTO>(request.Page_No, request.Page_Size);
                }

                response.Data = departments;
                response.Message = response.Data.Items.Count > 0 ? Messages.DataFound : Messages.NoDataFound;
                response.StatusCode = StausCodes.Accepted;
                response.IsSuccess = true;
            }
            catch (ValidationException exception)
            {
                response.Errors = exception.Errors.Select(err => err.ErrorMessage).ToList();
                response.Message = "";
                response.StatusCode = StausCodes.BadRequest;
                response.IsSuccess = false;
                return response;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.Message = Messages.IssueWithData;
                response.StatusCode = StausCodes.InternalServerError;
                response.IsSuccess = false;
                return response;
            }
            return response;
        }
    }
}