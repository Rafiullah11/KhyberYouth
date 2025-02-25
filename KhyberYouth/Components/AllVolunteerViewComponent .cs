using Microsoft.AspNetCore.Mvc;
using KhyberYouth.Models;
using KhyberYouth.ViewModel;

namespace KhyberYouth.ViewComponents
{
    public class AllVolunteerViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AllVolunteerViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int page = 1)
        {
            int pageSize = 6;
            // Fetch total count of volunteers
            var totalCount = _context.Volunteers.Count();

            // Fetch paginated volunteers
            var volunteers = _context.Volunteers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Map to ViewModel
            var volunteerViewModels = volunteers.Select(volunteer => new VolunteerViewModel
            {
                Id = volunteer.Id,
                Name = volunteer.Name,
                Qualification = volunteer.Qualification,
                Email = volunteer.Email,
                JoinedDate = volunteer.JoinedDate,
                PhoneNumber = volunteer.PhoneNumber,
                Address = volunteer.Address,
                ImagePath = volunteer.ImagePath
            }).ToList();

            // Create a PaginatedList view model
            var paginatedVolunteers = new PaginatedList<VolunteerViewModel>(
                volunteerViewModels, totalCount, page, pageSize);

            return View(paginatedVolunteers); // Pass PaginatedList to the view
        }
    }
}
