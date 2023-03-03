using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Type { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }
    }
}
