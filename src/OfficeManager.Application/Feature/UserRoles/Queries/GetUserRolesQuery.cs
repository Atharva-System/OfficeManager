using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.ApplicationRoles.Queries
{
    public record GetUserRolesQuery : IRequest<List<RolesDTO>>;

    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, List<RolesDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUserRolesQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RolesDTO>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Roles.ProjectTo<RolesDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
