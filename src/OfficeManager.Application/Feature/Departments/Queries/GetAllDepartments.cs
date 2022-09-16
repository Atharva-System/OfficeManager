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
    public record GetAllDepartments : IRequest<Response<List<DepartmentDTO>>>, ICacheable
    {
        public bool BypassCache => false;

        public string CacheKey => CacheKeys.Departments;
    }

    public class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartments, Response<List<DepartmentDTO>>>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;
        public GetAllDepartmentsHandler(IApplicationDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<Response<List<DepartmentDTO>>> Handle(GetAllDepartments request, CancellationToken cancellationToken)
        {
            Response<List<DepartmentDTO>> response = new Response<List<DepartmentDTO>>();
            try
            {

                List<DepartmentDTO> departments = new List<DepartmentDTO>();
               
                departments = await Context.Department
                    .AsNoTracking()
                    .ProjectTo<DepartmentDTO>(Mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
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
