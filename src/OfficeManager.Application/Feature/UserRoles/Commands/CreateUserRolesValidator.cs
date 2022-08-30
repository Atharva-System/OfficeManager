using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.UserRoles.Commands
{
    public class CreateUserRolesValidator : AbstractValidator<CreateUserRoles>
    {
        private readonly IApplicationDbContext _context;
        public CreateUserRolesValidator(IApplicationDbContext context)
        {
            _context = context;
            RuleFor(v => new UserRoleMapping { UserId = v.UserId, RoleId = v.RoleId })
                .MustAsync(BeUniqueRole).WithMessage("The specified role for this user already exists.");
        }

        public async Task<bool> BeUniqueRole(UserRoleMapping role, CancellationToken cancellationToken)
        {
            var userRole = await _context.UserRoleMapping.FirstOrDefaultAsync(x => x.UserId == role.UserId && x.RoleId == role.RoleId);
            return userRole is null;
        }
    }
}
