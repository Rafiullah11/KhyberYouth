using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace KhyberYouth.ViewModel
{
    public class BrandEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExistingImagePath { get; set; }

        [Display(Name = "Brand Image")]
        public IFormFile ImageFile { get; set; }
    }
}
