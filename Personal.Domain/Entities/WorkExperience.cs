using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class WorkExperience : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Position { get; set; }

        [Required]
        public int StartYear { get; set; }
        public int? EndYear { get; set; }

        [Required]
        public string RoleDescription { get; set; }
    }
}
