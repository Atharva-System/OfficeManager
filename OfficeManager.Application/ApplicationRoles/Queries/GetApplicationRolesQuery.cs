using MediatR;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.ApplicationRoles.Queries
{
    public record GetApplicationRolesQuery : IRequest<List<ApplicationRolesDto>>;

    public class GetApplicationRolesQueryHandler : IRequestHandler<GetApplicationRolesQuery, List<ApplicationRolesDto>>
    {
        private readonly IIdentityService _identityService;

        public GetApplicationRolesQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<ApplicationRolesDto>> Handle(GetApplicationRolesQuery request, CancellationToken cancellationToken)
        {
            return await _identityService.GetApplicationRoles();
        }
    }
}
