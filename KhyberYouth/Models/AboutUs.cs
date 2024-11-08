using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Models
{
    public class AboutUs
    {
        public int Id { get; set; }
        public string SectionTitle { get; set; }
        public string SectionContent { get; set; }
        public string ImagePath { get; set; } // Path to the image file

        [NotMapped] // This won't be mapped to the database
        public IFormFile ImageFile { get; set; } // For uploading files
    }
}
