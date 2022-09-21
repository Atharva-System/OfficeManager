using FluentValidation;
using OfficeManager.Application.Feature.Users.Commands;

namespace OfficeManager.Application.Feature.Users.Validators
{
    public class LoginUserValidator : AbstractValidator<LoginUser>
    {
        public LoginUserValidator()
        {
            RuleFor(p => p.EmployeeNo)
                .NotEmpty().WithMessage("Employee no is required")
                .MustAsync(IsNumericString).WithMessage("Please enter valid employee no.");
                

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password is required");
        }

        public async Task<bool> IsNumericString(string employeeNo,CancellationToken cancellationToken)
        {
            if(employeeNo == "0")
            {
                return false;
            }
            return int.TryParse(employeeNo, out var result);
        }
    }
}
