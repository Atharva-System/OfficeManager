using MediatR;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Feature.ApplicationUsers.Commands
{
    public record ForgotPassword : IRequest<bool>
    {
        public string Email { get; init; } = string.Empty;
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPassword, bool>
    {
        private readonly IApplicationDbContext context;

        public ForgotPasswordCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(ForgotPassword request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(true);
        }
    }
}
