using Ganss.Xss;
using HtmlAgilityPack;
using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<BlogController> _logger;

        public BlogController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<BlogController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public IActionResult AllBlogs()
        {
            var publishedBlogs = _context.BlogPosts
                    .Where(b => b.IsPublish) // Only fetch blogs where IsPublish is true
                    .OrderByDescending(b => b.PublishedOn) // Optional: Order by latest published
                    .ToList();

            return View(publishedBlogs);
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null) return NotFound();

            var recentBlogs = await _context.BlogPosts
                .Where(b => b.Id != id)
                .OrderByDescending(b => b.PublishedOn)
                .Take(3)
                .Select(b => new BlogViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    ShortDescription = string.IsNullOrEmpty(b.Contents) ? "No content available." : (b.Contents.Length > 50 ? b.Contents.Substring(0, 50) + "..." : b.Contents),
                    ImagePath = b.ImagePath
                })
                .ToListAsync();
            var blogUrl = Url.Action("Details", "Blog", new { id = blogPost.Id }, Request.Scheme);
            var viewModel = new BlogDetailsViewModel
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Contents = blogPost.Contents, // Sanitize before displaying
                PublishedOn = blogPost.PublishedOn,
                IsPublish = blogPost.IsPublish,
                ImagePath = blogPost.ImagePath,
                RecentBlogs = recentBlogs,
                BlogUrl = blogUrl // Pass the generated blog URL
            };

            return View(viewModel);
        }

       
    }
}
