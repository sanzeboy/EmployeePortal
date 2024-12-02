using EmployeePortal.Application.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace EmployeePortal.Application.Services.Emails
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // TODO : Add smpt email setting in appsettings
            return;
            try
            {
                using var smtpClient = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
                {
                    Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword),
                    EnableSsl = _emailSettings.EnableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log exception (implement logging as needed)
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }
    }

}
