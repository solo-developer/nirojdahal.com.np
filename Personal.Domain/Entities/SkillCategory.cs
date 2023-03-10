using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Personal.Domain.Entities
{
    public class SkillCategory : BaseEntity
    {
        public SkillCategory(string title, List<string> icons, List<string> skills)
        {
            Title = title;
            SetIcons(icons);
            skills.ForEach(a => Skills.Add(new Skill(a)));
        }
        public SkillCategory()
        {

        }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required, MaxLength(500)]
        public string Icons { get; private set; }

        public virtual List<Skill> Skills { get; set; } = new List<Skill>();

        public void SetIcons(List<string> icons)
        {
            this.Icons = string.Join(",", icons);
        }

        public List<string> GetIcons() => Icons.Split(",").ToList();
        
    }

    public class Skill : BaseEntity
    {
        public Skill(string name)
        {
            Name = name;
        }
        public Skill()
        {

        }

        [Required]
        public long CategoryId { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual SkillCategory Category { get; set; }
    }
}
