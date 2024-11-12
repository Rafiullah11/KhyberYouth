using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace KhyberYouth.Components
{
    public class IconsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public IconsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Fetch projects from the database
            var iconList = _context.MainIcons.ToList(); // Assuming you have a DbSet<Projects> in your context

            // Map to ViewModel
            var iconViewModels = iconList.Select(icon => new MainIconDetailsViewModel
            {
                Id = icon.Id,
                Title = icon.Title,
                Icons = icon.Icons,
                Description = icon.Description,
                ActionName = icon.ActionName,
                ControllerName=icon.ControllerName,
               
            }).ToList();

            return View(iconViewModels); // Pass ViewModel to the view
        }
    }
}
