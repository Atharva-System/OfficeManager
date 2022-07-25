using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Security;

namespace OfficeManager.Application.Departments.Queries.GetAllDepartmentsQuery
{
    [Authorize(Roles = "ADMINISTRATION")]
    public record GetAllDepartmentsQuery(string? search) : IRequest<List<DepartmentDto>>;

    public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, List<DepartmentDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetAllDepartmentsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.search))
                return await _context.DepartmentMasters.OrderBy(d => d.Name)
                        .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();

            return await _context.DepartmentMasters
                        .Where(d => d.Name.Contains(request.search))
                        .OrderBy(d => d.Name)
                        .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();
        }
    }
}
