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
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<BrandController> _logger;

        public BrandController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ILogger<BrandController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        // GET: Brand
        public async Task<IActionResult> Index()
        {
            var brands = await _context.Brands.ToListAsync();
            return View(brands);
        }

        // GET: Brand/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return NotFound();

            var viewModel = new BrandDetailsViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                ImagePath = brand.ImagePath
            };

            return View(viewModel);
        }

        // GET: Brand/Create
        public IActionResult Create() => View();

        // POST: Brand/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadFile(model.ImageFile);

                var brand = new Brand
                {
                    Name = model.Name,
                    ImagePath = uniqueFileName
                };

                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Brand/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return NotFound();

            var viewModel = new BrandEditViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                ExistingImagePath = brand.ImagePath
            };

            return View(viewModel);
        }

        // POST: Brand/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var brand = await _context.Brands.FindAsync(id);
                brand.Name = model.Name;

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
                    brand.ImagePath = UploadFile(model.ImageFile);
                }

                _context.Update(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Brand/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return NotFound();

            return View(brand);
        }

        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return NotFound();

            if (!string.IsNullOrEmpty(brand.ImagePath))
            {
                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", brand.ImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Brands.Remove(brand);
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
