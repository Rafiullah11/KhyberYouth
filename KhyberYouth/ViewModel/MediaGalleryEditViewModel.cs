using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.ViewModel
{
    public class MediaGalleryEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Property for the new uploaded photo
        public IFormFile? Photo { get; set; }

        // Property for displaying the existing image
        public string ExistingImagePath { get; set; }
    }
}
