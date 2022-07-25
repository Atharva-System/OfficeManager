using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.ApplicationRoles.Commands.CreateApplicationUserRoles
{
    public record CreateApplicationUserRolesCommand : IRequest<Result>
    {
        public string Name { get; set; }
    }

    public class CreateApplicationUserRolesCommandHandler : IRequestHandler<CreateApplicationUserRolesCommand,Result>
    {
        private readonly IIdentityService _identityService;
        
        public CreateApplicationUserRolesCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(CreateApplicationUserRolesCommand request,CancellationToken cancellationToken)
        {
            return await _identityService.CreateRoleAsync(request.Name);
        }
    }
}
