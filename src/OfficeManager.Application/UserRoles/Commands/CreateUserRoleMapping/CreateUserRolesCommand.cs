using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.ApplicationRoles.Commands.CreateApplicationUserRoles
{
    public record CreateUserRolesCommand : IRequest<Result>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class CreateUserRolesCommandHandler : IRequestHandler<CreateUserRolesCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserRolesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateUserRolesCommand request, CancellationToken cancellationToken)
        {
            _context.BeginTransaction();
            _context.UserRoleMapping.Add(new UserRoleMapping()
            {
                UserId = request.UserId,
                RoleId = request.RoleId
            });
            await _context.SaveChangesAsync(cancellationToken);
            _context.CommitTransaction();
            return Result.Success("Role Added Successfully",null);
        }
    }
}
