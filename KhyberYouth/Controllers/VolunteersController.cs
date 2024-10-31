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

namespace KhyberYouth.Controllers
{
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

        // GET: Volunteers
        public async Task<IActionResult> Index()
        {
            var volunteers = await _context.Volunteers.ToListAsync();
            return View(volunteers);
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
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Volunteers/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            var editViewModel = new VolunteerEditViewModel
            {
                Name = volunteer.Name,
                Qualification = volunteer.Qualification,
                Email = volunteer.Email,
                PhoneNumber = volunteer.PhoneNumber,
                Address = volunteer.Address,
                JoinedDate = volunteer.JoinedDate,
                ExistingImagePath = volunteer.ImagePath
            };

            return View(editViewModel);
        }

        // POST: Volunteers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VolunteerEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var volunteer = await _context.Volunteers.FindAsync(id);

                volunteer.Name = model.Name;
                volunteer.Qualification = model.Qualification;
                volunteer.Email = model.Email;
                volunteer.PhoneNumber = model.PhoneNumber;
                volunteer.Address = model.Address;
                volunteer.JoinedDate = model.JoinedDate;

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
                    volunteer.ImagePath = UploadFile(model.ImageFile);
                }

                _context.Update(volunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Volunteers/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(volunteer.ImagePath))
            {
                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", volunteer.ImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Volunteers.Remove(volunteer);
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
