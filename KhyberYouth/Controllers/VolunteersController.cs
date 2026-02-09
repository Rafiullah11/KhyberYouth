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
    public class VolunteersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<VolunteersController> _logger;

        public VolunteersController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ILogger<VolunteersController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public IActionResult AllVolunteer(int page = 1)
        {
            ViewData["Page"] = page; // Pass the page number to the view
            return View();
        }
        // GET: Volunteers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }

            var detailsViewModel = new VolunteerDetailsViewModel
            {
                Id = volunteer.Id,
                Name = volunteer.Name,
                Qualification = volunteer.Qualification,
                Email = volunteer.Email,
                PhoneNumber = volunteer.PhoneNumber,
                Address = volunteer.Address,
                JoinedDate = volunteer.JoinedDate,
                ImagePath=volunteer.ImagePath
                // Additional mapping for ImageFile if necessary
            };

            return View(detailsViewModel);
        }

        // GET: Volunteers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Volunteers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VolunteerCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadFile(model.ImageFile);

                var volunteer = new Volunteer
                {
                    Name = model.Name,
                    Qualification = model.Qualification,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    JoinedDate = model.JoinedDate,
                    ImagePath = uniqueFileName
                };

                _context.Add(volunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AllVolunteer));
            }
            return View(model);
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
