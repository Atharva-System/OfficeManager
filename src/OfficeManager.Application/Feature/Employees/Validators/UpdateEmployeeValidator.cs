﻿using FluentValidation;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Feature.Employees.Commands.Validator
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployee>
    {
        private readonly IApplicationDbContext Context;
        public UpdateEmployeeValidator(IApplicationDbContext context)
        {
            Context = context;

            RuleFor(p => p.employeeName)
                .NotEmpty().WithMessage("Employee Name is required")
                .MaximumLength(200).WithMessage("Employee name is exceeding the limit of 200 characters.");

            RuleFor(p => p.email)
                .NotEmpty().WithMessage("Email is required")
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
    }
}
