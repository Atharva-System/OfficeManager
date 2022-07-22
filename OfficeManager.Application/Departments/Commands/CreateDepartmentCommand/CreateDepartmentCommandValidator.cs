using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Departments.Commands.CreateDepartmentCommand
{
    public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
    {
        private readonly IApplicationDbContext _context;
        public CreateDepartmentCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            RuleFor(v => v.Name)
                .MaximumLength(100).WithMessage("Department name must not exceed 100 characters")
                .NotEmpty().WithMessage("Departname is required.")
                .MustAsync(BeUniqueDepartment).WithMessage("Specified department already exists");
        }

        public async Task<bool> BeUniqueDepartment(string department, CancellationToken cancellationToken)
        {
            return await _context.DepartmentMasters.AllAsync(d => d.Name != department, cancellationToken);
        }
    }
}
