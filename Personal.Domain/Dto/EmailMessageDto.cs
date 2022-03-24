using MimeKit;
using System.Collections.Generic;

namespace Personal.Domain.Dto
{
    public class EmailMessageDto
    {
        public EmailMessageDto()
        {

        }
        public EmailMessageDto(string subject, string content, string[] destinationAddresses)
        {
            To = new List<MailboxAddress>();
            for (var i = 0; i < destinationAddresses.Length; i++)
            {
                var address = destinationAddresses[i];
                To.Add(new MailboxAddress(address));
            }

            Subject = subject;
            Content = content;
        }
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

    }
}
