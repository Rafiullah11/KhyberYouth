using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.ViewModel
{
    public class BlogDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Contents { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishedOn { get; set; }
        public bool IsPublish { get; set; }
        public string? ImagePath { get; set; } // Path to the image file
        public string? ExistImage { get; set; } // Path to the image file

        [NotMapped] // This won't be mapped to the database
        public IFormFile? ImageFile { get; set; } // For uploading files
        public List<BlogViewModel> RecentBlogs { get; set; } // Add this
                                                             // New property for full URL
        public string BlogUrl { get; set; }

    }
}
