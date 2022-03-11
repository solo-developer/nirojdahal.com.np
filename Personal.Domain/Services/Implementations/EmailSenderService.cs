using MailKit.Net.Smtp;
using MimeKit;
using Personal.Domain.Configs;
using Personal.Domain.Constants;
using Personal.Domain.Dto;
using Personal.Domain.Services.Interface;

namespace Personal.Domain.Services.Implementations
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly ICryptoGraphyService _cryptoGraphyService;

        public EmailSenderService(EmailConfiguration emailConfig, ICryptoGraphyService cryptoGraphyService)
        {
            _emailConfig = emailConfig;
            _cryptoGraphyService = cryptoGraphyService;
        }

        public void SendEmail(EmailMessageDto message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessageDto message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    var password = _cryptoGraphyService.DecryptString(SecurityConstants.CRYPTO_KEY, _emailConfig.Password);
                    client.Authenticate(_emailConfig.UserName, password);
                    client.Send(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
