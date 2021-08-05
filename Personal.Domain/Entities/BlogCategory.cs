using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Personal.Domain.Entities
{
    public class BlogCategory : BaseEntity
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Slug is required.")]
        [MaxLength(200)]
        public string Slug { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public virtual List<Blog> Blogs { get; set; } = new List<Blog>();

        public bool HasBlogs() => Blogs.Any();
    }
}
