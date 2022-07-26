using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        private readonly IApplicationDbContext _context;
        public UpdateDepartmentCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            RuleFor(v => v.Name)
                .MaximumLength(100).WithMessage("Department name must not exceed 100 characters")
                .NotEmpty().WithMessage("Departname is required.");
            RuleFor(v => v.Description)
                .MaximumLength(500).WithMessage("Department description must not exceed 500 characters");
        }
    }
}
