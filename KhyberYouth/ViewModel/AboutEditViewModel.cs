using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace KhyberYouth.ViewModel
{
    public class AboutEditViewModel
    {
        public int Id { get; set; }
        public string SectionTitle { get; set; }
        public string SectionContent { get; set; }
        public string ExistingImagePath { get; set; }

        [Display(Name = "About Us Image")]
        public IFormFile ImageFile { get; set; }

    }
}
