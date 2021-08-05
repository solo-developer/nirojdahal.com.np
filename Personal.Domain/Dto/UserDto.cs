using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Personal.Domain.Dto
{
    public class UserDto
    {
        private string _imageName = "";

        public long UserId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        [MaxLength(75)]
        public string ImageName {
            get => _imageName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _imageName = "default-user.png";
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

        [MaxLength(50)]
        public string Address { get; set; }

        [MaxLength(15)]
        public string MobileNo { get; set; }


        [Required(AllowEmptyStrings =false,ErrorMessage ="Password is required.")]
        public string Password { get; set; }

        [Compare(nameof(Password),ErrorMessage ="Passwords didnot match.")]
        public string ConfirmPassword { get; set; }

    }
}
