namespace niroj.website.Areas.UserManagement.Models
{
    public class ChangePasswordViewModel
    {
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmPassword { get; set; }

        public bool IsPasswordMatching => newPassword.Equals(confirmPassword);
    }
}
