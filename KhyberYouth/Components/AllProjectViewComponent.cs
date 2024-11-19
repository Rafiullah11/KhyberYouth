using Microsoft.AspNetCore.Mvc;
using KhyberYouth.Models;
using KhyberYouth.ViewModel;

namespace KhyberYouth.ViewComponents
{
    public class AllProjectViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AllProjectViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int page = 1)
        {
            int pageSize = 3;
            // Fetch total count of projects
            var totalCount = _context.Projects.Count();

            // Fetch paginated projects
            var projects = _context.Projects
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Map to ViewModel
            var projectViewModels = projects.Select(project => new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                ShortDescription = project.Description,
                ImagePath = project.ImagePath
            }).ToList();

            // Create a PaginatedList view model
            var paginatedProjects = new PaginatedList<ProjectViewModel>(
                projectViewModels, totalCount, page, pageSize);

            return View(paginatedProjects); // Pass PaginatedList to the view
        }
    }
}
