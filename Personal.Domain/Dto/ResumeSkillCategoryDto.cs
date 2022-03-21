using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Dto
{
    public class ResumeSkillCategoryDto
    {
        public long Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
