using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace KhyberYouth.ViewModel
{
    public class ProjectEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsPublished { get; set; }
        public string ExistingImagePath { get; set; }

        [Display(Name = "Project Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
