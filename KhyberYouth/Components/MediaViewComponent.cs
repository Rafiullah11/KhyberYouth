using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace KhyberYouth.Components
{
    public class MediaViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MediaViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Fetch projects from the database
            var aboutUs = _context.MediaGalleries.ToList(); // Assuming you have a DbSet<Projects> in your context

            return View(aboutUs); // Pass ViewModel to the view
        }
    }
}
