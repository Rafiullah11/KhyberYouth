using KhyberYouth.Helpers;
using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.ViewModel
{
    public class VolunteerEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Qualification { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoinedDate { get; set; }

        public VolunteerStatus Status { get; set; } 
        public IFormFile ImageFile { get; set; }
        public string ExistingImagePath { get; set; }
    }
}
