using MediatR;
using OfficeManager.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.Feature.UserRoles.Commands
{
    public record DeleteUserRole(int id) : IRequest<IResponse>;

    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRole, IResponse>
    {
        private readonly IApplicationDbContext Context;
        public DeleteUserRoleCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<IResponse> Handle(DeleteUserRole request, CancellationToken cancellationToken)
        {
            Context.BeginTransaction();
            var userRole = await Context.UserRoleMapping.FirstOrDefaultAsync(d => d.Id == request.id);
            if (userRole != null)
            {
                Context.UserRoleMapping.Remove(userRole);
                await Context.SaveChangesAsync(cancellationToken);
                Context.CommitTransaction();
                return new SuccessResponse(StatusCodes.Accepted, Messages.DeletedSuccessfully);
            }
            return new ErrorResponse(StatusCodes.BadRequest,Messages.NoDataFound);
        }
    }
}
