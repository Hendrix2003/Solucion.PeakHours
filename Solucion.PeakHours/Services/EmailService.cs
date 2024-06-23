using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using SolucionPeakHours.Interfaces;
using SolucionPeakHours.Shared.UserAccount;

namespace SolucionPeakHours.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("MailSettings:SmtpUserName").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = request.Body
            };

            using var smtp = new SmtpClient();
            smtp.Connect(
                _config.GetSection("MailSettings:SmtpServer").Value,
                Convert.ToInt32(_config.GetSection("MailSettings:SmtpPort").Value),
                SecureSocketOptions.StartTls
            );

            smtp.Authenticate(_config.GetSection("MailSettings:SmtpUserName").Value, _config.GetSection("MailSettings:SmtpPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
