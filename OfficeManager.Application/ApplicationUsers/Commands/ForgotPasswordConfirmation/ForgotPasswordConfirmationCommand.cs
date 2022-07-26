using MediatR;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.ApplicationUsers.Commands.ForgotPasswordConfirmation
{
    public record ForgotPasswordConfirmationCommand : IRequest<bool>
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string Token { get; init; }
    }
    public class ForgotPasswordConfirmationCommandHandler : IRequestHandler<ForgotPasswordConfirmationCommand,bool>
    {
        private readonly IIdentityService _service;
        public ForgotPasswordConfirmationCommandHandler(IIdentityService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(ForgotPasswordConfirmationCommand request,CancellationToken cancellationToken)
        {
            return await _service.ForgotPasswordConfirmationAsync(request,cancellationToken);
        }
    }
}
