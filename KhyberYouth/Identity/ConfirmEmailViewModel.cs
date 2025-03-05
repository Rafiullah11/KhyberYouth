using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Identity
{
    public class ConfirmEmailViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
