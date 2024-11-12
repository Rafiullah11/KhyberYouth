using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace KhyberYouth.Components
{
    public class AboutViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AboutViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Fetch projects from the database
            var aboutUs = _context.AboutSections.ToList(); // Assuming you have a DbSet<Projects> in your context

            // Map to ViewModel
            var aboutViewModels = aboutUs.Select(about => new AboutDetailsViewModel
            {
                Id = about.Id,
                SectionTitle = about.SectionTitle,
                Subtitle = about.Subtitle,
                DescriptionTitle=about.DescriptionTitle,
                Description = about.Description,
                FounderName = about.FounderName,
                FounderTitle = about.FounderTitle,
                ExistingMainImagePath = about.MainImage,
                ExistingFounderImagePath = about.FounderImage
            }).ToList();

            return View(aboutViewModels); // Pass ViewModel to the view
        }
    }
}
