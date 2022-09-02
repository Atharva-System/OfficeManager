using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.UserRoles.Commands
{
    public record CreateUserRoles : IRequest<Result>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class CreateUserRolesCommandHandler : IRequestHandler<CreateUserRoles, Result>
    {
        private readonly IApplicationDbContext Context;

        public CreateUserRolesCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<Result> Handle(CreateUserRoles request, CancellationToken cancellationToken)
        {
            Context.BeginTransaction();
            await Context.UserRoleMapping.AddAsync(new UserRoleMapping()
            {
                UserId = request.UserId,
                RoleId = request.RoleId
            });
            await Context.SaveChangesAsync(cancellationToken);
            Context.CommitTransaction();
            return Result.Success(Messages.AddedSuccesfully, string.Empty);
        }
    }
}
