using MimeKit;
using System.Collections.Generic;

namespace Personal.Domain.Dto
{
    public class EmailMessageDto
    {
        public EmailMessageDto()
        {

        }
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public EmailMessageDto(string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.Add(new MailboxAddress("developer.niroj@gmail.com"));
            Subject = subject;
            Content = content;
        }
    }
}
