using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal.Domain.Entities
{
    public class ResumeSkill : BaseEntity
    {
        [Required]
        public long CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual ResumeSkillCategory Category { get; set; }
    }
}
