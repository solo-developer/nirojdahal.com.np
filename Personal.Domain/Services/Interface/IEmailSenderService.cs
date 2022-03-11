using Personal.Domain.Dto;

namespace Personal.Domain.Services.Interface
{
    public interface IEmailSenderService
    {
        void SendEmail(EmailMessageDto message);
    }
}
