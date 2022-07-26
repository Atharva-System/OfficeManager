using MediatR;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.ApplicationUsers.Commands.ForgotPassword
{
    public record ForgotPasswordCommand : IRequest<bool>
    {
        public string Email { get; init; }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IIdentityService _service;

        public ForgotPasswordCommandHandler(IIdentityService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request,CancellationToken cancellationToken)
        {
            return await _service.ForgotPasswordAsync(request.Email, cancellationToken);
        }
    }
}
