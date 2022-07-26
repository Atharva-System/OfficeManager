using OfficeManager.Application.Common.Models;
using MediatR;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.ApplicationRoles.Commands.DeleteUserRoles
{
    public record DeleteUserRoleCommand(string id) : IRequest<Result>;

    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand,Result>
    {
        private readonly IIdentityService _Service;
        public DeleteUserRoleCommandHandler(IIdentityService service)
        {
            _Service = service;
        }

        public async Task<Result> Handle(DeleteUserRoleCommand request,CancellationToken cancellationToken)
        {
            return await _Service.DeleteRoleAsync(request.id);
        }
    }
}
