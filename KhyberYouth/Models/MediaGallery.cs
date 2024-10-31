using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhyberYouth.Models
{
    public class MediaGallery
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; } // Path to the image file

        [NotMapped] // This won't be mapped to the database
        public IFormFile ImageFile { get; set; } // For uploading files
    }

}
