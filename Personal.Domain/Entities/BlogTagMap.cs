using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal.Domain.Entities
{
    public class BlogTagMap : BaseEntity
    {
        [Required]
        public long BlogId { get; set; }

        [Required]
        public long TagId { get; set; }

        [ForeignKey(nameof(BlogId))]
        public virtual Blog Blog { get; set; }

        [ForeignKey(nameof(TagId))]
        public virtual Tag Tag { get; set; }
    }
}
