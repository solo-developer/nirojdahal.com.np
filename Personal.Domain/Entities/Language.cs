using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class Language :BaseEntity
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
    }
}
