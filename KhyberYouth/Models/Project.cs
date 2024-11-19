using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhyberYouth.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public string ImagePath { get; set; } // Path to the image file

        [NotMapped] // This won't be mapped to the database
        public IFormFile? ImageFile { get; set; } // For uploading files
    }
}
