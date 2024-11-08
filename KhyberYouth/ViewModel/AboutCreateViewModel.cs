using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhyberYouth.ViewModel
{
    public class AboutCreateViewModel
    {
        [Required]
        public string SectionTitle { get; set; }
        public string SectionContent { get; set; }

        [Display(Name = "Brand Image")]
        public IFormFile ImageFile { get; set; }

    }
}
