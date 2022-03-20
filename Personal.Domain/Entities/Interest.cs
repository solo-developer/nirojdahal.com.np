using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class Interest : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
