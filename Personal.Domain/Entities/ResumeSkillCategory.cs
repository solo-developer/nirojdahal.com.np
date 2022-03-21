using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class ResumeSkillCategory : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        public bool IsDeleted { get; set; }

        public virtual List<ResumeSkill> Skills { get; set; }=new List<ResumeSkill>();

        public void  Delete()=> IsDeleted = true;
    }
}
