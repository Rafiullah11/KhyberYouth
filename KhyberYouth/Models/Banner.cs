using System.ComponentModel.DataAnnotations.Schema;

namespace KhyberYouth.Models
{
    public class Banner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; } // Path to the image file

        [NotMapped] // This won't be mapped to the database
        public IFormFile ImageFile { get; set; } // For uploading files
    }
}
