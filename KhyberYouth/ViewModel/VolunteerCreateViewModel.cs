using KhyberYouth.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhyberYouth.ViewModel
{
    public class VolunteerCreateViewModel
    {
        public string Name { get; set; }
        public string Qualification { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public VolunteerStatus Status { get; set; } = VolunteerStatus.Pending;

        [DataType(DataType.Date)]
        public DateTime JoinedDate { get; set; }
        public IFormFile ImageFile { get; set; } 
    }
}
