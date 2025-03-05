using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Identity
{
    public class ChangePasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password), Compare("Password", ErrorMessage = "Password and confirm not match")]
        public string ConfirmPassword { get; set; }
    }
}
