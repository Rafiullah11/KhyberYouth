using System.ComponentModel.DataAnnotations.Schema;

namespace KhyberYouth.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Qualification { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime JoinedDate { get; set; }
        public string ImagePath { get; set; } // Path to the image file

        [NotMapped] // This won't be mapped to the database
        public IFormFile ImageFile { get; set; } // For uploading files
    }
}
