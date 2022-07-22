using MediatR;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Common.Security;

namespace OfficeManager.Application.UserRoles.Command.DeleteUserRoles
{

    [Authorize(Roles = "ADMINISTRATION")]
    public record DeleteUserRoleCommand(Guid Id) : IRequest<Result>;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public DeleteUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            var userRole = _context.UserRole.FirstOrDefault(r => r.Id == request.Id);
            if (userRole == null)
            {
                throw new NotFoundException();
            }

            _context.UserRole.Remove(userRole);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success("Role deleted successfully.");
        }
    }
}
