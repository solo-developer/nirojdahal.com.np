using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class Module : BaseEntity
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public virtual List<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
