using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.Common.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message, CancellationToken cancellationToken);
        Task SendEmailAsync(Message message, CancellationToken cancellationToken);
    }
}
