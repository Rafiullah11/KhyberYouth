using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.ViewModel
{
    public class BannerCreateViewModel
    {
            [Required]
            public string Name { get; set; }

            [Display(Name = "Brand Image")]
            public IFormFile ImageFile { get; set; }
        
    }
}
