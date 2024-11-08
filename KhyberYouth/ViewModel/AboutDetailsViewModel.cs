using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhyberYouth.ViewModel
{
    public class AboutDetailsViewModel
    {
       
        public int Id { get; set; }
        public string SectionTitle { get; set; }
        public string SectionContent { get; set; }
        public string ImagePath { get; set; } // Path to the image file
        public string ExistingImagePath { get; set; }

    }
}
