using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.ViewModel
{
    public class BlogCreateViewModel
    {
        public string Title { get; set; }
        public string? Contents { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishedOn { get; set; }
        public bool IsPublish { get; set; }
        public string? ImagePath { get; set; } // Path to the image file

        [NotMapped] // This won't be mapped to the database
        public IFormFile? ImageFile { get; set; } // For uploading files
    }
}