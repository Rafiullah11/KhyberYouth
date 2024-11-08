using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace KhyberYouth.ViewModel
{
    public class BannerEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExistingImagePath { get; set; }

        [Display(Name = "Banner Image")]
        public IFormFile ImageFile { get; set; }
    }
}
