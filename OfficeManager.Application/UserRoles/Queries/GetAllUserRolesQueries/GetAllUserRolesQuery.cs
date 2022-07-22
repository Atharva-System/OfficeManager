using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Security;

namespace OfficeManager.Application.UserRoles.Queries.GetAllUserRolesQueries
{
    public record GetAllUserRolesQuery(string role) : IRequest<List<UserRoleDto>>;

    public class GetAllUserRolesQueryHandler : IRequestHandler<GetAllUserRolesQuery, List<UserRoleDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllUserRolesQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserRoleDto>> Handle(GetAllUserRolesQuery request, CancellationToken cancellationToken)
        {
            if(!string.IsNullOrEmpty(request.role))
            {
                return await _context.UserRole
                    .AsNoTracking()
                    .ProjectTo<UserRoleDto>(_mapper.ConfigurationProvider)
                    .Where(r => r.Title.Contains(request.role))
                    .OrderBy(r => r.Title)
                    .ToListAsync(cancellationToken);
            }
            return await _context.UserRole
                    .AsNoTracking()
                    .ProjectTo<UserRoleDto>(_mapper.ConfigurationProvider)
                    .OrderBy(r => r.Title)
                    .ToListAsync(cancellationToken);
        }
    }
}
