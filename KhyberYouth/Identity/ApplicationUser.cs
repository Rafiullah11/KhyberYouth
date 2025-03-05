using Microsoft.AspNetCore.Identity;

namespace KhyberYouth.Identity
{
    public class ApplicationUser : IdentityUser
    {
		public string? FullName { get; set; }

    }
}