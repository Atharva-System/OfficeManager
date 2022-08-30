using OfficeManager.Application.Common.Models;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OfficeManager.Application.Feature.UserRoles.Commands
{
    public record DeleteUserRole(int id) : IRequest<Result>;

    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRole, Result>
    {
        private readonly IApplicationDbContext context;
        public DeleteUserRoleCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Result> Handle(DeleteUserRole request, CancellationToken cancellationToken)
        {
            context.BeginTransaction();
            var userRole = await context.UserRoleMapping.FirstOrDefaultAsync(d => d.Id == request.id);
            if (userRole != null)
            {
                context.UserRoleMapping.Remove(userRole);
                await context.SaveChangesAsync(cancellationToken);
                context.CommitTransaction();
                return Result.Success("Role deleted Successfully", string.Empty);
            }
            else
            {
                return Result.Failure(new List<string>() { "No Role found" }, string.Empty);
            }
        }
    }
}
