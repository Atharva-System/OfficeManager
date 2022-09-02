using OfficeManager.Application.Common.Models;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OfficeManager.Application.Feature.UserRoles.Commands
{
    public record DeleteUserRole(int id) : IRequest<Result>;

    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRole, Result>
    {
        private readonly IApplicationDbContext Context;
        public DeleteUserRoleCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<Result> Handle(DeleteUserRole request, CancellationToken cancellationToken)
        {
            Context.BeginTransaction();
            var userRole = await Context.UserRoleMapping.FirstOrDefaultAsync(d => d.Id == request.id);
            if (userRole != null)
            {
                Context.UserRoleMapping.Remove(userRole);
                await Context.SaveChangesAsync(cancellationToken);
                Context.CommitTransaction();
                return Result.Success(Messages.DeletedSuccessfully, string.Empty);
            }
            else
            {
                return Result.Failure(new List<string>() { Messages.NoDataFound }, string.Empty);
            }
        }
    }
}
