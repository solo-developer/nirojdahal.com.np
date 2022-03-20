using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class Tag : BaseEntity
    {
        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

        public bool IsDeleted { get;protected set; }

        public virtual List<BlogTagMap> Blogs { get; set; }=new List<BlogTagMap>();
        public void MarkDeleted() => IsDeleted = true;
    }
}
