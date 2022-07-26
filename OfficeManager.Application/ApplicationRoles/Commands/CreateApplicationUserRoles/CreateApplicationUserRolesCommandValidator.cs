using FluentValidation;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.ApplicationRoles.Commands.CreateApplicationUserRoles
{
    public class CreateApplicationUserRolesCommandValidator : AbstractValidator<CreateApplicationUserRolesCommand>
    {
        private readonly IIdentityService _service;
        public CreateApplicationUserRolesCommandValidator(IIdentityService service)
        {
            _service = service;
            RuleFor(v => v.Name)
                .MustAsync(BeUniqueRole).WithMessage("The specified role already exists.");
        }

        public async Task<bool> BeUniqueRole(string roleName,CancellationToken cancellationToken)
        {
            return !(await _service.RoleExists(roleName));
        }
    }
}
