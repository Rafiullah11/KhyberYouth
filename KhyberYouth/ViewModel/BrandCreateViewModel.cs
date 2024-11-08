using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.ViewModel
{
    public class BrandCreateViewModel
    {
            [Required]
            public string Name { get; set; }

            [Display(Name = "Brand Image")]
            public IFormFile ImageFile { get; set; }
        
    }
}
