using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class Education : BaseEntity
    {
        [Required]
        [MaxLength(150)]
        public string Faculty { get; set; }

        [Required]
        [MaxLength(150)]
        public string InstitutionName { get; set; }

        [Required]
        public int StartYear { get; set; }
        public int?  EndYear { get; set; }
    }
}
