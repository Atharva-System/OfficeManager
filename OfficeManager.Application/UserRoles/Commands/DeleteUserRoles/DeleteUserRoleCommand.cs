using OfficeManager.Application.Common.Models;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OfficeManager.Application.ApplicationRoles.Commands.DeleteUserRoles
{
    public record DeleteUserRoleCommand(int id) : IRequest<Result>;

    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        public DeleteUserRoleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteUserRoleCommand request,CancellationToken cancellationToken)
        {
            _context.BeginTransaction();
            var userRole = await _context.UserRoleMapping.Where(d => d.Id == request.id).FirstOrDefaultAsync();
            _context.UserRoleMapping.Remove(userRole);
            await _context.SaveChangesAsync(cancellationToken);
            _context.CommitTransaction();
            return Result.Success("Role deleted Successfully");
        }
    }
}
