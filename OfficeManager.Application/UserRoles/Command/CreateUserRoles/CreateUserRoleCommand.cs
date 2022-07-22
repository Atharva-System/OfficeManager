using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Common.Security;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.UserRoles.Command.CreateUserRoles
{
    [Authorize(Roles = "ADMINISTRATION")]
    public record CreateUserRoleCommand : IRequest<Result>
    {
        public string Title { get; init; }
        public string Description { get; init; }
    }

    public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserRoleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
        {
            UserRole role = new UserRole
            {
                Title = request.Title,
                Description = request.Description
            };

            _context.UserRole.Add(role);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success($" {role.Id} Role created successfully.");
        }
    }
}
