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
        private readonly IApplicationDbContext _context;
        public ForgotPasswordConfirmationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ForgotPasswordConfirmationCommand request,CancellationToken cancellationToken)
        {
            return true;
        }
    }
}
