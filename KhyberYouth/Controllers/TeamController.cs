using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace KhyberYouth.Controllers
{
    [AllowAnonymous]
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<TeamController> _logger;

        public TeamController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ILogger<TeamController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }
        public IActionResult AllTeam(int page = 1)
        {
            ViewData["Page"] = page; // Pass the page number to the view
            return View();
        }
        // GET: Team/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember == null)
            {
                return NotFound();
            }

            var detailsViewModel = new TeamDetailsViewModel 
            {
                Id = teamMember.Id,
                Name = teamMember.Name,
                Qualification = teamMember.Qualification,
                Email = teamMember.Email,
                PhoneNumber = teamMember.PhoneNumber,
                Address = teamMember.Address,
                JoinedDate = teamMember.JoinedDate,
                ImagePath = teamMember.ImagePath,
                Dept=teamMember.Dept,
                // You can map additional properties here if needed
            };

            return View(detailsViewModel);
        }

    }
}
