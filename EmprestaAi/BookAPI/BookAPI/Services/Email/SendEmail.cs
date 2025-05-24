using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.Extensions.Configuration;

namespace BookAPI.Services.Email
{
    public class SendEmail : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _username;
        private readonly string _password;
        private readonly string _host;
        private readonly int _port;

        public SendEmail(IConfiguration configuration)
        {
            _configuration = configuration;
            _username = _configuration["SMTP:UserName"] ?? throw new ArgumentNullException("SMTP:UserName is missing");
            _password = _configuration["SMTP:Password"] ?? throw new ArgumentNullException("SMTP:Password is missing");
            _host = _configuration["SMTP:Host"] ?? throw new ArgumentNullException("SMTP:Host is missing");
            _port = int.TryParse(_configuration["SMTP:Port"], out int port) ? port : 587;
        }

        public async Task EnviarEmailAsync(IEnumerable<string> emailsTo, string subject, string body, IEnumerable<string> attachments)
        {
            var message = PrepareMessage(emailsTo, subject, body, attachments);
            await SendEmailBySmtpAsync(message);
        }

        private MailMessage PrepareMessage(IEnumerable<string> emailsTo, string subject, string body, IEnumerable<string> attachments)
        {
            var mail = new MailMessage { From = new MailAddress(_username) };

            foreach (var email in emailsTo)
            {
                if (ValidateEmail(email))
                {
                    mail.To.Add(email);
                }
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            foreach (var attachment in attachments)
            {
                var data = new Attachment(attachment, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = File.GetCreationTime(attachment);
                disposition.ModificationDate = File.GetLastWriteTime(attachment);
                disposition.ReadDate = File.GetLastAccessTime(attachment);
                mail.Attachments.Add(data);
            }

            return mail;
        }

        private bool ValidateEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

        private async Task SendEmailBySmtpAsync(MailMessage message)
        {
            using var smtpClient = new SmtpClient(_host)
            {
                Port = _port,
                EnableSsl = true,
                Timeout = 50000,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_username, _password)
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}
