using System;
using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class ContactUs : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }


        [Required]
        [MaxLength(500)]
        public string Comment { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
