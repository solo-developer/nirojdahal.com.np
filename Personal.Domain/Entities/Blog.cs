using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Personal.Domain.Entities
{
    public class Blog : BaseEntity
    {

        public long? CategoryId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Slug is required.")]
        [MaxLength(200)]
        public string Slug { get; set; }

        [Required]
        [MaxLength(70)]
        public string BannerImage { get; set; }


        [MaxLength(1500)]
        public string ShortDescription { get; set; }


        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required]
        public bool IsPublished { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(CreatedBy))]
        public virtual ApplicationUser Creator { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual BlogCategory Category { get; set; }

        public virtual List<BlogComment> Comments { get; set; } = new List<BlogComment>();
        public virtual List<BlogTagMap> Tags { get; set; } = new List<BlogTagMap>();

        internal void HideFromView(string performedBy)
        {
            ModifiedBy = performedBy;
            ModifiedDate = DateTime.Now;
            IsPublished = false;
        }

        internal void ShowInView(string performedBy)
        {
            ModifiedBy = performedBy;
            ModifiedDate = DateTime.Now;
            IsPublished = true;
        }

        internal void Delete(string performedBy)
        {
            ModifiedBy = performedBy;
            ModifiedDate = DateTime.Now;
            IsDeleted = true;
        }
    }
}
