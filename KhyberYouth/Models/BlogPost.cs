﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhyberYouth.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Contents { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishedOn { get; set; }
        public bool IsPublish { get; set; }
        public string? ImagePath { get; set; } // Path to the image file

        [NotMapped] // This won't be mapped to the database
        public IFormFile? ImageFile { get; set; } // For uploading files
    }
}
