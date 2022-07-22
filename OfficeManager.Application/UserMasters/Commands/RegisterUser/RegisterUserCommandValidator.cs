using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.UserMasters.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly IApplicationDbContext _context;
        public RegisterUserCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(300).WithMessage("Email  must not exceed 300 characters.")
                .MustAsync(BeUniqueEmail).WithMessage("The specified email already exists.");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be atleast 8 characters long.");

            RuleFor(v => v.PersonalEmail)
                .NotEmpty().WithMessage("Personal Email is required")
                .MaximumLength(300).WithMessage("Personal Email  must not exceed 300 characters.");

            RuleFor(v => v.Contact)
                .NotEmpty().WithMessage("Contact is required")
                .MaximumLength(20).WithMessage("Contact  must not exceed 20 characters.");
        }

        public async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return await _context.UserMaster.AllAsync(u => u.Email != email, cancellationToken);
        }
    }
}
