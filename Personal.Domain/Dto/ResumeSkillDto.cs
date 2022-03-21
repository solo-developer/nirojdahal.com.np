using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Dto
{
    public class ResumeSkillDto
    {
        public long Id { get; set; }

        [Required]
        public long CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string CategoryName { get; set; }
    }
}
