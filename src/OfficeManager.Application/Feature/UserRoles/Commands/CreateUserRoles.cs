using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.UserRoles.Commands
{
    public record CreateUserRoles : IRequest<IResponse>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class CreateUserRolesCommandHandler : IRequestHandler<CreateUserRoles, IResponse>
    {
        private readonly IApplicationDbContext Context;

        public CreateUserRolesCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<IResponse> Handle(CreateUserRoles request, CancellationToken cancellationToken)
        {
            Context.BeginTransaction();
            await Context.UserRoleMapping.AddAsync(new UserRoleMapping()
            {
                UserId = request.UserId,
                RoleId = request.RoleId
            });
            await Context.SaveChangesAsync(cancellationToken);
            Context.CommitTransaction();
            return new SuccessResponse(StatusCodes.Accepted, Messages.AddedSuccesfully);
        }
    }
}
