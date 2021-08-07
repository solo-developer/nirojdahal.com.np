using System;
using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Dto
{
    public class ContactUsDto
    {
        private DateTime _date = default;
        private bool _isUnread = false;

        [Required]
        public string Recaptcha { get; set; }
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


        internal void SetDate(DateTime val)
        {
            _date = val;
        }
        public void SetUnread()
        {
            _isUnread = true;
        }

        public bool IsUnread() => _isUnread;

        public DateTime GetDate() => _date;
    }
}
