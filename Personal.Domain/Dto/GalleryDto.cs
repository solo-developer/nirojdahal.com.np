using Personal.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Dto
{
    public class GalleryDto
    {
        private string _title, _description;

        public long Id { get; set; }

        [Display(Name = "Image")]
        public string ImageName { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Please enter a title.", AllowEmptyStrings = false)]
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NonEmptyValueException("Title is required.");
                }
                _title = value;
            }
        }

        [Display(Name = "Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a description")]
        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NonEmptyValueException("Description is required.");
                }
                _description = value;
            }
        }

        [Display(Name = "Status")]
        public bool IsEnabled { get; set; } = true;

        [Display(Name = "Mark Slider Image")]
        public bool IsSliderImage { get; set; } = true;
    }
}
