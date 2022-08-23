﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Employees.Commands.AddEmployee
{
    public class AddEmployeeCommandValidator : AbstractValidator<AddEmployeeCommand>
    {
        private readonly IApplicationDbContext _context;
        public AddEmployeeCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(p => p.employeeName)
                .NotEmpty().WithMessage("Employee Name is required")
                .MaximumLength(200).WithMessage("Employee name is exceeding the limit of 200 characters.");

            RuleFor(p => p.email)
                .NotEmpty().WithMessage("Email is required")
                .MustAsync(BeUniqueEmail).WithMessage("Specified email already exist..")
                .MaximumLength(200).WithMessage("Email is exceeding the limit of 200 characters.");

            RuleFor(p => p.departmentId)
                .NotEmpty().WithMessage("Department is required.");

            RuleFor(p => p.designationId)
                .NotEmpty().WithMessage("Designation is required");

            RuleFor(p => p.dateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required");

            RuleFor(p => p.dateOfJoining)
                .NotEmpty().WithMessage("Date of Joining is required");
        }

        public async Task<bool> BeUniqueEmail(string email,CancellationToken cancellationToken)
        {
            return !(await _context.Employees.AnyAsync(emp => emp.Email == email));
        }
    }
}