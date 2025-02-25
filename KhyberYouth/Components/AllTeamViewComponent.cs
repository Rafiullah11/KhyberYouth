using Microsoft.AspNetCore.Mvc;
using KhyberYouth.Models;
using KhyberYouth.ViewModel;

namespace KhyberYouth.ViewComponents
{
    public class AllTeamViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AllTeamViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int page = 1)
        {
            int pageSize = 6;

            var totalCount = _context.TeamMembers.Count();

            var teamMembers = _context.TeamMembers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Map to ViewModel
            var teamViewModels = teamMembers.Select(team => new TeamDetailsViewModel
            {
               Id = team.Id,
               Name = team.Name,
               Qualification = team.Qualification,
               PhoneNumber = team.PhoneNumber,
               JoinedDate = team.JoinedDate,
               Email = team.Email,
               Dept = team.Dept,
               Address = team.Address,
               ImagePath = team.ImagePath
            }).ToList();

            // Create a PaginatedList view model
            var paginatedTeams = new PaginatedList<TeamDetailsViewModel>(
                teamViewModels, totalCount, page, pageSize);

            return View(paginatedTeams); // Pass PaginatedList to the view
        }
    }
}
