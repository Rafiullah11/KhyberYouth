using KhyberYouth.Models;
using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.ViewModel
{
    public class TeamCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Qualification { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoinedDate { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ImageFile { get; set; }

        [Required]
        [Display(Name = "Department")]
        public Dept Dept { get; set; }
    }

}
