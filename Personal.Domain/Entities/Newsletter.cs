using System;
using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class Newsletter : BaseEntity
    {
        [Required]
        [MaxLength(75)]
        public string Email { get; set; }

        [Required]
        public DateTime SubscribedDate { get; set; } = DateTime.Now;

    }
}
