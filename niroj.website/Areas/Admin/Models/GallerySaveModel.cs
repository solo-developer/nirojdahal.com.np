using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace niroj.website.Areas.Admin.Models
{
    public class GallerySaveModel
    {
        public long Id { get; set; }

        public string ImageName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Page title is required")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }


        [Display(Name = "Is Slider")]
        public bool IsSliderImage { get; set; }

        [Display(Name = "Status")]
        public bool IsEnabled { get; set; } = true;

        public IFormFile File { get; set; }
    }
}
