using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Infrastructure.Settings;

namespace OfficeManager.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            this.emailSettings = options.Value;
        }

        public void SendEmail(Message message, CancellationToken cancellationToken)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage, cancellationToken);
        }

        public async Task SendEmailAsync(Message message, CancellationToken cancellationToken)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage, cancellationToken);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(emailSettings.From, emailSettings.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodybuilder = new BodyBuilder { HtmlBody = String.Format("<h2 style='color:red;'>{0}</h1>", message.Content) };
            if (message.Attachements != null && message.Attachements.Any())
            {
                byte[] fileBytes;
                foreach (var attachement in message.Attachements)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachement.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodybuilder.Attachments.Add(attachement.Name, fileBytes, ContentType.Parse(attachement.ContentType));
                }
            }
            emailMessage.Body = bodybuilder.ToMessageBody();

            return emailMessage;
        }

        private async void Send(MimeMessage mailMessage, CancellationToken cancellationToken)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(emailSettings.SmtpServer, emailSettings.Port, true, cancellationToken);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(emailSettings.Username, emailSettings.Password, cancellationToken);
                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true, cancellationToken);
                    client.Dispose();
                }
            };
        }

        private async Task SendAsync(MimeMessage mailMessage, CancellationToken cancellationToken)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(emailSettings.SmtpServer, emailSettings.Port, true, cancellationToken);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(emailSettings.Username, emailSettings.Password, cancellationToken);
                await client.SendAsync(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception, or both.
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true, cancellationToken);
                client.Dispose();
            }
        }
    }
}
