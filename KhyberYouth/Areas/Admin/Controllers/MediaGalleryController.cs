using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace KhyberYouth.Areas.Admin.Controllers
{
    [Authorize] // Restricts access to authenticated users
    [Area("Admin")]
    public class MediaGalleryController : Controller
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MediaGalleryController> _logger;

        public MediaGalleryController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnvironment, ApplicationDbContext context, ILogger<MediaGalleryController> logger)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
            _logger = logger;
        }

        //[AllowAnonymous]
        public IActionResult Index()
        {
           var list = _context.MediaGalleries.ToList();
            return View(list);
        }

        public IActionResult Media()
        {
            return View();
        }


        [HttpGet("Details/{id}")]
        //[AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogTrace("Fetching details for media item.");
            var media = await _context.MediaGalleries.FindAsync(id);

            if (media == null)
            {
                Response.StatusCode = 404;
                return View("MediaNotFound", id);
            }

            MediaGalleryDetailsViewModel viewModel = new MediaGalleryDetailsViewModel
            {
                Description = media.Description,
                ImagePath = media.ImagePath,
                Title = media.Title,
            };

            return View(viewModel);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(MediaGalleryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Photo.CopyTo(fileStream);
                    }
                }

                MediaGallery newMediaItem = new MediaGallery
                {
                    Title = model.Title,
                    Description = model.Description,
                    ImagePath = uniqueFileName
                };

                _context.Add(newMediaItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var media = await _context.MediaGalleries.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }

            MediaGalleryEditViewModel editViewModel = new MediaGalleryEditViewModel
            {
                Id = media.Id,
                Title = media.Title,
                Description = media.Description,
                ExistingImagePath = media.ImagePath
            };

            return View(editViewModel);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(MediaGalleryEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var media = await _context.MediaGalleries.FindAsync(model.Id);

                media.Title = model.Title;
                media.Description = model.Description;

                if (model.Photo != null)
                {
                    if (model.ExistingImagePath != null)
                    {
                        string oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, "images", model.ExistingImagePath);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    media.ImagePath = PhotoUploadMethod(model);
                }

                _context.Update(media);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var media = await _context.MediaGalleries.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }

            return View(media);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var media = await _context.MediaGalleries.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }

            _context.MediaGalleries.Remove(media);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private string PhotoUploadMethod(MediaGalleryEditViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}











//using KhyberYouth.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace KhyberYouth.Controllers
//{
//    public class MediaGalleryController : Controller
//    {
//        public IActionResult Index()
//        {
//            // Dummy data for example purposes
//            var mediaList = new List<MediaGallery>
//        {
//            new MediaGallery { Id = 1, Title = "Picture 1", Description = "Description for Picture 1", ImagePath = "/images/p1.jpg" },
//            new MediaGallery { Id = 2, Title = "Picture 2", Description = "Description for Picture 2", ImagePath = "/images/p2.jpg" },
//            new MediaGallery { Id = 1, Title = "Picture 1", Description = "Description for Picture 1", ImagePath = "/images/p1.jpg" },
//            new MediaGallery { Id = 2, Title = "Picture 2", Description = "Description for Picture 2", ImagePath = "/images/p2.jpg" },
//            new MediaGallery { Id = 3, Title = "Picture 3", Description = "Description for Picture 3", ImagePath = "/images/p3.jpg" },
//            new MediaGallery { Id = 3, Title = "Picture 3", Description = "Description for Picture 3", ImagePath = "/images/p3.jpg" }
//        };

//            return View(mediaList);
//        }
//    }
//}
