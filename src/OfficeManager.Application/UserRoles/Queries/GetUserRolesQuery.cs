using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.ApplicationRoles.Queries
{
    public record GetUserRolesQuery : IRequest<List<RolesDto>>;

    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, List<RolesDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUserRolesQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RolesDto>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Roles.ProjectTo<RolesDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
