using Microsoft.AspNetCore.Mvc;
using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using System.Linq;

namespace KhyberYouth.ViewComponents
{
    public class AllVolunteerViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AllVolunteerViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Fetch all volunteers without pagination
            var volunteers = _context.Volunteers.ToList();

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

            return View(volunteerViewModels); // Return full list to the view
        }
    }
}
