using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Models
{
    public class AboutSection
    {
        public int Id { get; set; }
        public string SectionTitle { get; set; }
        public string Subtitle { get; set; }
        public string DescriptionTitle { get; set; }
        public string Description { get; set; }
        public string MainImage { get; set; }

        [NotMapped] 
        public IFormFile MainImageFile { get; set; } 
        public string FounderImage { get; set; }

        [NotMapped] 
        public IFormFile FounderImageFile { get; set; } 
        public string FounderName { get; set; }
        public string FounderTitle { get; set; }
    }
}
