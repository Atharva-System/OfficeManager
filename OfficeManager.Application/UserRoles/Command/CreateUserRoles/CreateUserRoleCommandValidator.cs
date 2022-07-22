using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.UserRoles.Command.CreateUserRoles
{
    public class CreateUserRoleCommandValidator : AbstractValidator<CreateUserRoleCommand>
    {
        private readonly IApplicationDbContext _context;
        public CreateUserRoleCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Title)
                .MaximumLength(200).WithMessage("Role title can not exceed limit of 200 characters.")
                .NotEmpty().WithMessage("Role title is required.")
                .MustAsync(BeUniqueRole).WithMessage("Specified role already exists.");

        }

        public async Task<bool> BeUniqueRole(string role, CancellationToken cancellationToken)
        {
            return await _context.UserRole.AllAsync(r => r.Title != role, cancellationToken);
        }
    }
}
