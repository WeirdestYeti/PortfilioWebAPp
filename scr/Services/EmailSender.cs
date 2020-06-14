using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using PortfolioWebApp.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PortfolioWebApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailerSettingsOptions _mailerOptions;

        private readonly SmtpClient _smtpClient;

        public EmailSender(IOptionsSnapshot<MailerSettingsOptions> mailerOptions)
        {
            _mailerOptions = mailerOptions.Value;

            _smtpClient = new SmtpClient
            {
                Host = _mailerOptions.Host,
                Port = _mailerOptions.Port,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_mailerOptions.CredentialUserName, _mailerOptions.CredentialUserName),
                EnableSsl = true
            };
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute( subject, message, email);
        }

        public async Task Execute(string subject, string message, string email)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_mailerOptions.CredentialUserName);
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
