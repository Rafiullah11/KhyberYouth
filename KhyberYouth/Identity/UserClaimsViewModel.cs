using System.Security.Claims;

namespace KhyberYouth.Identity
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
                Claims = new List<UserClaim>();
        }
        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }
    }
}
