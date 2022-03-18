using Personal.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Entities
{
    public class Gallery : BaseEntity
    {
        private string _title, _description, _imageName;

        [Required]
        [MaxLength(70)]
        public string ImageName
        {
            get => _imageName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NonEmptyValueException("Image is required.");
                }
                _imageName = value;
            }
        }

        [Required]
        [MaxLength(70)]
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

        [Required]
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

        public bool IsEnabled { get; set; } = true;

        public bool IsSliderImage { get; set; } = false;

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }

        public void MarkSliderImage()
        {
            IsSliderImage = true;
        }

        public void RemoveFromSliderImage()
        {
            IsSliderImage = false;
        }
    }
}
