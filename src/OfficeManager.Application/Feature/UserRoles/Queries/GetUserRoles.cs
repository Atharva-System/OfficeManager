using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.ApplicationRoles.Queries
{
    public record GetUserRoles : IRequest<IResponse>;

    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRoles, IResponse>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;

        public GetUserRolesQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<IResponse> Handle(GetUserRoles request, CancellationToken cancellationToken)
        {
            return new DataResponse<List<RolesDTO>>(await Context.Roles.ProjectTo<RolesDTO>(Mapper.ConfigurationProvider).ToListAsync(),StatusCodes.Accepted,Messages.DataFound);
        }
    }
}
