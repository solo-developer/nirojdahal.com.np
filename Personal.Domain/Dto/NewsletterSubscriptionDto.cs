using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Dto
{
    public class NewsletterSubscriptionDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
