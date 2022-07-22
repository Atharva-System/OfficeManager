using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;
using OfficeManager.Domain.Events;

namespace OfficeManager.Application.UserMasters.Commands.RegisterUser
{
    public record RegisterUserCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string PersonalEmail { get; set; }
        public DateTime DateOfJoining { get; set; } = DateTime.Now;
        public Guid RoleId { get; set; }
    }

    public class RegisterUserCommandHadnler : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public RegisterUserCommandHadnler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new UserMaster
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = request.RoleId,
                Profile = new UserProfile
                {
                    Contact = request.Contact,
                    PersonalEmail = request.PersonalEmail,
                    DateOfJoining = request.DateOfJoining
                }
            };

            user.AddDomainEvent(new EmployeeRegisteredEvent(user));
            
            _context.UserMaster.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("User registered successfully");
        }
    }
}
