using System.ComponentModel.DataAnnotations;

namespace niroj.website.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email is required")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is required")]
        [MaxLength (100)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
