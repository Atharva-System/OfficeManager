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
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;

        public GetUserRolesQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<List<RolesDTO>> Handle(GetUserRoles request, CancellationToken cancellationToken)
        {
            return await Context.Roles.ProjectTo<RolesDTO>(Mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
