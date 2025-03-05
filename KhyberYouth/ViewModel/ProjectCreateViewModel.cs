using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.ViewModel
{
    public class ProjectCreateViewModel
    {
            [Required]
            public string Name { get; set; }

            public string Description { get; set; }

            [DataType(DataType.Date)]
            public DateTime StartDate { get; set; }

            [DataType(DataType.Date)]
            public DateTime EndDate { get; set; }

            public bool IsCompleted { get; set; }
            public bool IsPublished { get; set; }

            [Display(Name = "Project Image")]
            public IFormFile? ImageFile { get; set; }
        
    }
}
