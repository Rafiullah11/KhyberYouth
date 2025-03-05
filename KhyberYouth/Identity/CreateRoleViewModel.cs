using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Identity
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
