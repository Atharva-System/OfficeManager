using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Interfaces;

namespace OfficeManager.Application.Feature.Departments.Queries
{
    public record SearchDepartments : IRequest<Response<PaginatedList<DepartmentDTO>>>
    {
        public string search { get; init; } = string.Empty;
        public int Page_No { get; set; } = 1;
        public int Page_Size { get; set; } = 10;
    }
    public class SearchDepartmentHandler : IRequestHandler<SearchDepartments,Response<PaginatedList<DepartmentDTO>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SearchDepartmentHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedList<DepartmentDTO>>> Handle(SearchDepartments request, CancellationToken cancellationToken)
        {
            Response<PaginatedList<DepartmentDTO>> response = new Response<PaginatedList<DepartmentDTO>>();
            try
            {
                PaginatedList<DepartmentDTO> departments = new PaginatedList<DepartmentDTO>(new List<DepartmentDTO>(),0,request.Page_No, request.Page_Size);
                if(string.IsNullOrEmpty(request.search))
                {
                    departments = await _context.Department
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
