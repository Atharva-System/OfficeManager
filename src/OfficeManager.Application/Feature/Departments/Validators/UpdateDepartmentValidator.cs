using FluentValidation;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Feature.Employees.Commands.Validator
{
    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartment>
    {
        private readonly IApplicationDbContext Context;

        public UpdateDepartmentValidator(IApplicationDbContext context)
        {
            Context = context;
            RuleFor(p => p.id)
               .NotEmpty().WithMessage("id is required");

            RuleFor(p => p.name)
                .NotEmpty().WithMessage("name is required")
                .MaximumLength(50).WithMessage("name is exceeding the limit of 50 characters.");

            RuleFor(p => p.description)
                .MaximumLength(200).WithMessage("description is exceeding the limit of 200 characters.");
        }
    }
}