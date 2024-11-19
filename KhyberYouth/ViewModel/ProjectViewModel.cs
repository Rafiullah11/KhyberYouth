namespace KhyberYouth.ViewModel
{
    public class ProjectViewModel
    {
        public int Id { get; set; } // For linking to the project details page
        public string Name { get; set; } // Project title
        public string ShortDescription { get; set; } // Brief description for the card
        public string ImagePath { get; set; } // Path to the project's image
    }
}
