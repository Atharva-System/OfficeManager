using OfficeManager.Application.Common.Models;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OfficeManager.Application.Feature.UserRoles.Commands
{
    public record DeleteUserRole(int id) : IRequest<Result>;

    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRole, Result>
    {
        private readonly IApplicationDbContext _context;
        public DeleteUserRoleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteUserRole request, CancellationToken cancellationToken)
        {
            _context.BeginTransaction();
            var userRole = await _context.UserRoleMapping.FirstOrDefaultAsync(d => d.Id == request.id);
            if (userRole != null)
            {
                _context.UserRoleMapping.Remove(userRole);
                await _context.SaveChangesAsync(cancellationToken);
                _context.CommitTransaction();
                return Result.Success("Role deleted Successfully", string.Empty);
            }
            else
            {
                return Result.Failure(new List<string>() { "No Role found" }, string.Empty);
            }
        }
    }
}
