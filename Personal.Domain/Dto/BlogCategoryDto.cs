using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Personal.Domain.Dto
{
    public class BlogCategoryDto
    {
        public long Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        public string Slug { get;protected set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public string PerformedBy { get; set; }

        internal void SetSlug(string slug)
        {
            Slug=slug;
        }
    }
}
