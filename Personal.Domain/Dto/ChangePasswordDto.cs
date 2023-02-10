namespace Personal.Domain.Dto
{
    public class ChangePasswordDto
    {
        public string  AspUserId { get; set; }
        public string CurrentPassword { get; set; }
        public string Password { get; set; }

    }
}
