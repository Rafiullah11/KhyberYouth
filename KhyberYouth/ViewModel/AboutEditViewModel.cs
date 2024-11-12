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
        public string Subtitle { get; set; }
        public string DescriptionTitle { get; set; }
        public string Description { get; set; }

        [Display(Name = "MainImage Image")]
        public IFormFile MainImageFile { get; set; }

        [Display(Name = "FounderImage Image")]
        public IFormFile FounderImageFile { get; set; }
        public string FounderName { get; set; }
        public string FounderTitle { get; set; }

        public string? ExistingMainImagePath { get; set; }
        public string? ExistingFounderImagePath { get; set; }
    }
}
