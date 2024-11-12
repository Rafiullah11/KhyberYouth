using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using System.Threading.Tasks;

namespace KhyberYouth.Controllers
{
    public class AboutSectionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<AboutSectionController> _logger;

        public AboutSectionController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ILogger<AboutSectionController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        // GET: AboutSections
        public async Task<IActionResult> Index()
        {
            var aboutSections = await _context.AboutSections.ToListAsync();
            return View(aboutSections);
        }


        public async Task<IActionResult> AboutUs()
        {
            return View();
        }

        // GET: AboutSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutSection = await _context.AboutSections.FindAsync(id);
            if (aboutSection == null)
            {
                return NotFound();
            }

            var detailsViewModel = new AboutDetailsViewModel
            {
                Id = aboutSection.Id,
                SectionTitle=aboutSection.SectionTitle,
                Subtitle = aboutSection.Subtitle,
                DescriptionTitle=aboutSection.DescriptionTitle,
                Description = aboutSection.Description,
                FounderName = aboutSection.FounderName,
                FounderTitle = aboutSection.FounderTitle,
                ExistingMainImagePath = aboutSection.MainImage,
                
                ExistingFounderImagePath = aboutSection.FounderImage
            };

            return View(detailsViewModel);
        }

        // GET: AboutSections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AboutSections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string mainImageFileName = null;
                string founderImageFileName = null;

                // Check if main image file is uploaded
                if (model.MainImageFile != null && model.MainImageFile.Length > 0)
                {
                    mainImageFileName = UploadFile(model.MainImageFile);
                }

                // Check if founder image file is uploaded
                if (model.FounderImageFile != null && model.FounderImageFile.Length > 0)
                {
                    founderImageFileName = UploadFile(model.FounderImageFile);
                }

                var aboutSection = new AboutSection
                {
                    SectionTitle=model.SectionTitle,
                    Subtitle = model.Subtitle,
                    DescriptionTitle = model.DescriptionTitle,
                    Description = model.Description,
                    FounderName = model.FounderName,
                    FounderTitle = model.FounderTitle,
                    MainImage = mainImageFileName,  // Will be null if no image is uploaded
                    FounderImage = founderImageFileName  // Will be null if no image is uploaded
                };
                
                _context.Add(aboutSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AboutSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutSection = await _context.AboutSections.FindAsync(id);
            if (aboutSection == null)
            {
                return NotFound();
            }

            var editViewModel = new AboutEditViewModel
            {
                Id = aboutSection.Id,
                SectionTitle = aboutSection.SectionTitle,
                Subtitle = aboutSection.Subtitle,
                DescriptionTitle = aboutSection.DescriptionTitle,
                Description = aboutSection.Description,
                FounderName = aboutSection.FounderName,
                FounderTitle = aboutSection.FounderTitle,
                ExistingMainImagePath = aboutSection.MainImage,
                ExistingFounderImagePath = aboutSection.FounderImage
            };

            return View(editViewModel);
        }

        // POST: AboutSections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AboutEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var aboutSection = await _context.AboutSections.FindAsync(id);

                if (aboutSection == null)
                {
                    return NotFound();
                }
                aboutSection.SectionTitle = model.SectionTitle;
                aboutSection.Subtitle = model.Subtitle;
                aboutSection.DescriptionTitle = model.DescriptionTitle;
                aboutSection.Description = model.Description;
                aboutSection.FounderName = model.FounderName;
                aboutSection.FounderTitle = model.FounderTitle;

                if (model.MainImageFile != null)
                {
                    if (!string.IsNullOrEmpty(model.ExistingMainImagePath))
                    {
                        string oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, "images", model.ExistingMainImagePath);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    aboutSection.MainImage = UploadFile(model.MainImageFile);
                }

                if (model.FounderImageFile != null)
                {
                    if (!string.IsNullOrEmpty(model.ExistingFounderImagePath))
                    {
                        string oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, "images", model.ExistingFounderImagePath);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    aboutSection.FounderImage = UploadFile(model.FounderImageFile);
                }

                _context.Update(aboutSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: AboutSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutSection = await _context.AboutSections.FindAsync(id);
            if (aboutSection == null)
            {
                return NotFound();
            }

            return View(aboutSection);
        }

        // POST: AboutSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aboutSection = await _context.AboutSections.FindAsync(id);
            if (aboutSection == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(aboutSection.MainImage))
            {
                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", aboutSection.MainImage);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            if (!string.IsNullOrEmpty(aboutSection.FounderImage))
            {
                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", aboutSection.FounderImage);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.AboutSections.Remove(aboutSection);
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
