using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal.Domain.Entities
{
    public class BlogComment : BaseEntity
    {
        [Required]
        public long BlogId { get; set; }

        public long ParentCommentId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Slug is required.")]
        [MaxLength(200)]
        public string Slug { get; set; }

        [MaxLength(100)]
        public string UserName { get; set; }
        public bool IsSystemUser { get; set; } = false;

        [Required(AllowEmptyStrings =false,ErrorMessage ="Comment is required.")]
        [MaxLength(1000)]
        public string Comment { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [ForeignKey(nameof(BlogId))]
        public virtual Blog Blog { get; set; }

        [ForeignKey(nameof(ParentCommentId))]
        public virtual BlogComment ParentComment { get; set; }

        public virtual List<BlogComment> Replies { get; set; } = new List<BlogComment>();
    }
}
