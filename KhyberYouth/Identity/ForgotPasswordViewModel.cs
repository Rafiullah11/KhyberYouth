using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Identity
{
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
