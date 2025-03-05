using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace KhyberYouth.Controllers
{
    //[Authorize]
    [AllowAnonymous]
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

        public IActionResult Media()
        {
            return View();
        }

        [HttpGet("Details/{id}")]
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

       
    }
}

