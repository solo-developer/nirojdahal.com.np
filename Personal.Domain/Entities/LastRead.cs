using Personal.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal.Domain.Entities
{
    public class LastRead : BaseEntity
    {
        [Required]
        public ReadableTableKeys Key { get; set; }

        [Required]
        public DateTime ReadDate { get; set; } = DateTime.Now;

        [Required]
        public string ReadBy { get; set; }

        [ForeignKey(nameof(ReadBy))]
        public virtual ApplicationUser User { get; set; }
    }
}
