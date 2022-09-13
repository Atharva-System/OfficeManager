using FluentValidation;
using OfficeManager.Application.Feature.ApplicationUsers.Commands;

namespace OfficeManager.Application.Feature.ApplicationUsers.Validators
{
    public class LoginUserValidator : AbstractValidator<LoginUser>
    {
        public LoginUserValidator()
        {
            RuleFor(p => p.EmployeeNo)
                .NotEmpty().WithMessage("Employee no is required")
                .GreaterThan(0).WithMessage("Please enter valida employee no");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
