using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;

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
        private readonly ICurrentUserService _currentUserService;
        public LoginApplicationUserCommandHandler(IIdentityService service, ICurrentUserService currentUserService)
        {
            _service = service;
            _currentUserService = currentUserService;
        }

        public async Task<LoggedInUserDto> Handle(LoginApplicationUserCommand request, CancellationToken cancellationToken)
        {
             var result = await _service.LoginAsync(request.UserName, request.Password);
            if(result.UserName != null)
            {
                _currentUserService.UserId = result.UserId;
                return result;
            }
            return null;
        }
    }
}
