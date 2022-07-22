using MediatR;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.UserMasters.Commands.AuthenticateUser
{
    public class AuthenticateUserCommand : IRequest<LoggedInUserDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, LoggedInUserDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AuthenticateUserCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<LoggedInUserDto?> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _context.UserMaster.FirstOrDefault(u => u.Email == request.Email);
            var result = new LoggedInUserDto();
            
            if (user == null)
            {
                throw new NotFoundException("Username or password is not currect");
            }

            if (BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                user.Profile = _context.UserProfile.FirstOrDefault(p => p.UserId == user.Id);

                _currentUserService.UserId = user.Id;
                result.UserId = user.Id;
                result.Email = user.Email;
                result.Role = user.Role;
                result.Contact = user.Profile.Contact;

                return result;
            }

            return null;
        }
    }
}
