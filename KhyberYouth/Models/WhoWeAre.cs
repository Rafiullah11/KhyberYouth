using System.ComponentModel.DataAnnotations.Schema;

namespace KhyberYouth.Models
{
    public class WhoWeAre
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Dscription { get; set; }

        [NotMapped]
        public IFormFile MainImageFile { get; set; }
        public string? MainImage { get; set; }
    }
}
