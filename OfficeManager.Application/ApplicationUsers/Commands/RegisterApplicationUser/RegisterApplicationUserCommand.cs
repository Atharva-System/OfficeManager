using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.ApplicationUsers.Commands.RegisterApplicationUser
{
    public record RegisterApplicationUserCommand : IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string roleId { get; set; }
        public string PersonalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonalPhoneNumber { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class RegisterApplicationUserCommandHandler : IRequestHandler<RegisterApplicationUserCommand,Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public RegisterApplicationUserCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Result> Handle(RegisterApplicationUserCommand request, CancellationToken cancellationToken)
        {
            UserProfile profile = new UserProfile
            {
                PersonalEmail = request.PersonalEmail,
                Contact = request.PersonalPhoneNumber,
                DateOfBirth = request.DateOfBirth,
                DateOfJoining = request.DateOfJoining,
                ProfilePic = ""
            };

            _context.UserProfile.Add(profile);
            await _context.SaveChangesAsync(cancellationToken);

            ApplicationUser user = new ApplicationUser
            {
                UserName = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _identityService.CreateAsync(user, request.roleId, request.Password);
            result.result.Message = $"{result.userId} is registered successfully.";

            return result.result;
        }
    }
}
