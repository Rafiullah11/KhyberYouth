using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace KhyberYouth.Components
{
    public class TeamViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public TeamViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var teamMembers = _context.TeamMembers.ToList(); // Assuming you have a DbSet<teamMember> in your context

            // Map to ViewModel
            var teamtViewModels = teamMembers.Select(team => new TeamDetailsViewModel
            {
                Id = team.Id,
                Name = team.Name,
                Qualification = team.Qualification,
                Email = team.Email,
                PhoneNumber = team.PhoneNumber,
                Address = team.Address,
                JoinedDate = team.JoinedDate,
                ImagePath=team.ImagePath,
                Dept=team.Dept,
            }).ToList();

            return View(teamtViewModels); 
        }
    }
}