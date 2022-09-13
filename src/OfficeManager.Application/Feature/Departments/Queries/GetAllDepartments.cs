using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Interfaces;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.Feature.Departments.Queries
{
    public record GetAllDepartments : IRequest<IResponse>, ICacheable
    {
        public bool BypassCache => false;

        public string CacheKey => CacheKeys.Departments;
    }

    public class GetAllDepartmentsHandler : IRequestHandler<GetAllDepartments, IResponse>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;
        public GetAllDepartmentsHandler(IApplicationDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<IResponse> Handle(GetAllDepartments request, CancellationToken cancellationToken)
        {
            try
            {

                List<DepartmentDTO> departments = new List<DepartmentDTO>();
               
                departments = await Context.Department
                    .AsNoTracking()
                    .ProjectTo<DepartmentDTO>(Mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return new DataResponse<List<DepartmentDTO>>(departments, 200);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
