using KhyberYouth.Models;
using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.ViewModel
{
    public class TeamDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Qualification { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoinedDate { get; set; }
        public string ImagePath { get; set; }

        [Display(Name = "Department")]
        public Dept Dept { get; set; }
    }

}
