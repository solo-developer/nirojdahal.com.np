using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Dto
{
    public class BlogDto
    {
        public long Id { get; set; }
        public long? CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public string Slug { get; protected set; }

        [MaxLength(1500)]
        public string ShortDescription { get; set; }

        public string BannerImage { get; set; }

        public IFormFile Banner { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }

        public string PerformedBy { get; set; }

        public DateTime  CreatedDate { get;private set; }

        public bool IsPublished { get; set; }

        public List<long> Tags { get; set; } = new List<long>();

        public List<string> TagNames { get; set; }=new List<string>();

        public void SetSlug(string slug)
        {
            Slug = slug;
        }

        public void SetDate(DateTime date)
        {
            CreatedDate = date;
        }
    }
}
