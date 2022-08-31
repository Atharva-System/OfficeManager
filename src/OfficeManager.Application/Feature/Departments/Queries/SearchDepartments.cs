using AutoMapper;
using AutoMapper.QueryableExtensions;
using MailKit.Mime;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;

namespace OfficeManager.Application.Feature.Departments.Queries
{
    public record SearchDepartments(string search) : IRequest<Response<List<DepartmentDTO>>>;

    public class SearchDepartmentQueryHandler : IRequestHandler<SearchDepartments, Response<List<DepartmentDTO>>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public SearchDepartmentQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Response<List<DepartmentDTO>>> Handle(SearchDepartments request, CancellationToken cancellationToken)
        {
            Response<List<DepartmentDTO>> response = new Response<List<DepartmentDTO>>();
            try
            {

                List<DepartmentDTO> departments = new List<DepartmentDTO>();
                if (!string.IsNullOrEmpty(request.search))
                {
                    departments = await context.Department
                        .AsNoTracking()
                        .ProjectTo<DepartmentDTO>(mapper.ConfigurationProvider)
                        .Where(x => x.Name.Contains(request.search)).ToListAsync(cancellationToken);
                }
                else
                {
                    departments = await context.Department
                        .ProjectTo<DepartmentDTO>(mapper.ConfigurationProvider)
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
