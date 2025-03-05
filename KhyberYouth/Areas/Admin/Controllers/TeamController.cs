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

namespace KhyberYouth.Areas.Admin.Controllers
{
    [Authorize] // Restricts access to authenticated users
    [Area("Admin")]
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

        // GET: Team
        public async Task<IActionResult> Index()
        {
            var teamMembers = await _context.TeamMembers.ToListAsync();
            return View(teamMembers);
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

        // GET: Team/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Team/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadFile(model.ImageFile);

                var teamMember = new TeamMember
                {
                    Name = model.Name,
                    Qualification = model.Qualification,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    JoinedDate = model.JoinedDate,
                    ImagePath = uniqueFileName,
                    Dept = model.Dept
                };

                _context.Add(teamMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Team/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            var editViewModel = new TeamEditViewModel
            {
                Id = teamMember.Id, // Make sure to set the ID
                Name = teamMember.Name,
                Qualification = teamMember.Qualification,
                Email = teamMember.Email,
                PhoneNumber = teamMember.PhoneNumber,
                Address = teamMember.Address,
                JoinedDate = teamMember.JoinedDate,
                ExistingImagePath = teamMember.ImagePath, // Assuming this property exists in the ViewModel
                Dept = teamMember.Dept
        };

            return View(editViewModel);
        }

        // POST: Team/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var teamMember = await _context.TeamMembers.FindAsync(id);

                if (teamMember == null)
                {
                    return NotFound();
                }

                teamMember.Name = model.Name;
                teamMember.Qualification = model.Qualification;
                teamMember.Email = model.Email;
                teamMember.PhoneNumber = model.PhoneNumber;
                teamMember.Address = model.Address;
                teamMember.JoinedDate = model.JoinedDate;

                if (model.ImageFile != null)
                {
                    if (!string.IsNullOrEmpty(model.ExistingImagePath))
                    {
                        string oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, "images", model.ExistingImagePath);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    teamMember.ImagePath = UploadFile(model.ImageFile);
                }

                _context.Update(teamMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Team/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(teamMember);
        }

        // POST: Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(teamMember.ImagePath))
            {
                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", teamMember.ImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private string UploadFile(IFormFile imageFile)
        {
            string uniqueFileName = null;

            if (imageFile != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
