using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Feature.Employees.Commands.Validator
{
    public class AddDepartmentValidator : AbstractValidator<AddDepartment>
    {
        private readonly IApplicationDbContext Context;

        public AddDepartmentValidator(IApplicationDbContext context)
        {
            Context = context;

            RuleFor(p => p.name)
                .NotEmpty().WithMessage("name is required")
                .MaximumLength(50).WithMessage("name is exceeding the limit of 50 characters.")
                .MustAsync(BeUniqueDepartment).WithMessage("Specified Department already exist..");

            RuleFor(p => p.description)
                .MaximumLength(200).WithMessage("description is exceeding the limit of 200 characters.");
        }

        public async Task<bool> BeUniqueDepartment(string name, CancellationToken cancellationToken)
        {
            return !await Context.Department.AnyAsync(dep => dep.Name == name);
        }
    }
}