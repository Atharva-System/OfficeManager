using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.ApplicationRoles.Queries
{
    public record GetUserRoles : IRequest<List<RolesDTO>>;

    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRoles, List<RolesDTO>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetUserRolesQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<RolesDTO>> Handle(GetUserRoles request, CancellationToken cancellationToken)
        {
            return await context.Roles.ProjectTo<RolesDTO>(mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
