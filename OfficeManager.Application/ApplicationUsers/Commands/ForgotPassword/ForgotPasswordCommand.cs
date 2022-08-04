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
        private readonly IApplicationDbContext _context;

        public ForgotPasswordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request,CancellationToken cancellationToken)
        {
            return true;
        }
    }
}
