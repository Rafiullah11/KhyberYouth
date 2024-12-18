﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhyberYouth.ViewModel
{
    public class AboutCreateViewModel
    {
        [Required]
        public string SectionTitle { get; set; }
        public string Subtitle { get; set; }
        public string DescriptionTitle { get; set; }
        public string Description { get; set; }
        [Display(Name = "Main Picture")]
        public IFormFile MainImageFile { get; set; } 

        [Display(Name = "Founder Picture")]
        public IFormFile FounderImageFile { get; set; } 
        public string FounderName { get; set; }
        public string FounderTitle { get; set; }

    }
}
