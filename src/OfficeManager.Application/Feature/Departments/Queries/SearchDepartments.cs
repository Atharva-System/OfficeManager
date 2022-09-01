using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Interfaces;

namespace OfficeManager.Application.Feature.Departments.Queries
{
    public record SearchDepartments : IRequest<Response<List<DepartmentDTO>>>, ICacheable
    {
        public string Search { get; set; } = string.Empty;

        public bool BypassCache => false;

        public string CacheKey => CacheKeys.Departments;
    }

    public class SearchDepartmentQueryHandler : IRequestHandler<SearchDepartments, Response<List<DepartmentDTO>>>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;
        public SearchDepartmentQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<Response<List<DepartmentDTO>>> Handle(SearchDepartments request, CancellationToken cancellationToken)
        {
            Response<List<DepartmentDTO>> response = new Response<List<DepartmentDTO>>();
            try
            {

                List<DepartmentDTO> departments = new List<DepartmentDTO>();
                if (!string.IsNullOrEmpty(request.Search))
                {
                    departments = await Context.Department
                        .AsNoTracking()
                        .ProjectTo<DepartmentDTO>(Mapper.ConfigurationProvider)
                        .Where(x => x.Name.Contains(request.Search)).ToListAsync(cancellationToken);
                }
                else
                {
                    departments = await Context.Department
                        .ProjectTo<DepartmentDTO>(Mapper.ConfigurationProvider)
                        .ToListAsync();
                }
                response.Data = departments;
                response.StatusCode = StausCodes.Accepted;
                response.IsSuccess = true;
                response.Message = departments.Count > 0 ? Messages.DataFound : Messages.NoDataFound;

                return response;
            }
            catch (Exception ex)
            {
                response.Data = new List<DepartmentDTO>();
                response.Message = Messages.IssueWithData;
                response.Errors.Add(ex.Message);
                response.IsSuccess = false;
                response.StatusCode = StausCodes.InternalServerError;
                return response;
            }


        }
    }
}
