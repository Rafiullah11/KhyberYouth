using Microsoft.AspNetCore.Mvc;
using KhyberYouth.Models; 
using KhyberYouth.ViewModel;

namespace KhyberYouth.ViewComponents
{
    public class ProjectViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context; 

        public ProjectViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Fetch projects from the database
            //var projects = _context.Projects.ToList(); // Assuming you have a DbSet<Projects> in your context

            var projects = _context.Projects
               .Where(projet => projet.IsPublished) // Only include published projets
               .OrderByDescending(projet => projet.Id) // Optional: Sort by latest published
               .ToList();

            // Map to ViewModel
            var projectViewModels = projects.Select(project => new ProjectDetailsViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = (DateTime)project.EndDate,
                IsCompleted = project.IsCompleted,
                ImagePath = project.ImagePath
            }).ToList();

            return View(projectViewModels); // Pass ViewModel to the view
        }
    }
}
