using MediatR;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Feature.ApplicationUsers.Commands
{
    public record ForgotPasswordConfirmation : IRequest<bool>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
    }
    public class ForgotPasswordConfirmationCommandHandler : IRequestHandler<ForgotPasswordConfirmation, bool>
    {
        private readonly IApplicationDbContext context;
        public ForgotPasswordConfirmationCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(ForgotPasswordConfirmation request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(true);
        }
    }
}
