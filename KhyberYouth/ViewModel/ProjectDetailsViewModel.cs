using System;

namespace KhyberYouth.ViewModel
{
    public class ProjectDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public string ImagePath { get; set; }
        public List<ProjectViewModel> RecentProjects { get; set; } // Add this
    }
}
