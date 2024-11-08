using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KhyberYouth.Controllers
{
    public class BannerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<BannerController> _logger;

        public BannerController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ILogger<BannerController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        // GET: Banner
        public async Task<IActionResult> Index()
        {
            var Banners = await _context.Banners.ToListAsync();
            return View(Banners);
        }

        // GET: Banner/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var Banner = await _context.Banners.FindAsync(id);
            if (Banner == null) return NotFound();

            var viewModel = new BannerDetailsViewModel
            {
                Id = Banner.Id,
                Name = Banner.Name,
                ImagePath = Banner.ImagePath
            };

            return View(viewModel);
        }

        // GET: Banner/Create
        public IActionResult Create() => View();

        // POST: Banner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadFile(model.ImageFile);

                var Banner = new Banner
                {
                    Name = model.Name,
                    ImagePath = uniqueFileName
                };

                _context.Add(Banner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Banner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var Banner = await _context.Banners.FindAsync(id);
            if (Banner == null) return NotFound();

            var viewModel = new BannerEditViewModel
            {
                Id = Banner.Id,
                Name = Banner.Name,
                ExistingImagePath = Banner.ImagePath
            };

            return View(viewModel);
        }

        // POST: Banner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BannerEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var Banner = await _context.Banners.FindAsync(id);
                Banner.Name = model.Name;

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
                    Banner.ImagePath = UploadFile(model.ImageFile);
                }

                _context.Update(Banner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Banner/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var Banner = await _context.Banners.FindAsync(id);
            if (Banner == null) return NotFound();

            return View(Banner);
        }

        // POST: Banner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Banner = await _context.Banners.FindAsync(id);
            if (Banner == null) return NotFound();

            if (!string.IsNullOrEmpty(Banner.ImagePath))
            {
                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", Banner.ImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Banners.Remove(Banner);
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
