using MediatR;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.ApplicationUsers.Commands.LoginApplicationUser
{
    public record LoginApplicationUserCommand : IRequest<LoggedInUserDto>
    {
        public string UserName { get; init; }
        public string Password { get; init; }
    }

    public class LoginApplicationUserCommandHandler : IRequestHandler<LoginApplicationUserCommand,LoggedInUserDto>
    {
        private readonly IIdentityService _service;
        public LoginApplicationUserCommandHandler(IIdentityService service)
        {
            _service = service;
        }

        public async Task<LoggedInUserDto> Handle(LoginApplicationUserCommand request, CancellationToken cancellationToken)
        {
            return await _service.LoginAsync(request.UserName, request.Password);
        }
    }
}
