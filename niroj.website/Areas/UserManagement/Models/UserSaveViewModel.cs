using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace niroj.website.Areas.UserManagement.Models
{
    public class UserSaveViewModel
    {
        private string _imageName = "";

        public long UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(75)]
        public string ImageName
        {
            get => _imageName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _imageName = "default-user.jpg";
                }
                else
                {
                    _imageName = value;
                }
            }
        }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(15)]
        public string MobileNo { get; set; }

       // [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords didnot match.")]
        public string ConfirmPassword { get; set; }

        public IFormFile Image { get; set; }
    }
}
