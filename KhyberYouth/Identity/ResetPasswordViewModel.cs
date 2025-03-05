using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Identity
{
    public class ResetPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        //[MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        // Optional for password reset scenarios
        public string? Token { get; set; }
    }
}
