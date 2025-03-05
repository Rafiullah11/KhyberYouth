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
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;

namespace KhyberYouth.Areas.Admin.Controllers
{
    [Authorize] // Restricts access to authenticated users
    [Area("Admin")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ILogger<ProjectsController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        // GET: Projects
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var projects = _context.Projects.AsNoTracking();

            int pageSize = 4;

            return View(await PaginatedList<Project>.CreateAsync(projects, pageNumber ?? 1, pageSize));
        }

        public IActionResult AllProject()
        {
            var publishedBlogs = _context.Projects
          .Where(b => b.IsPublished) // Only fetch blogs where IsPublish is true
          .OrderByDescending(b => b.Id) // Optional: Order by latest published
          .ToList();

            return View(publishedBlogs);
        }



        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            // Fetch recent projects, excluding the current project
            var recentProjects = await _context.Projects
                .Where(p => p.Id != id && p.IsPublished)
                .OrderByDescending(p => p.StartDate)
                .Take(3)
                .Select(p => new ProjectViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShortDescription = string.IsNullOrEmpty(p.Description)
                        ? "No description available."
                        : (p.Description.Length > 50
                            ? p.Description.Substring(0, 50) + "..."
                            : p.Description),
                    ImagePath = p.ImagePath
                })
                .ToListAsync();

            var viewModel = new ProjectDetailsViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = (DateTime)project.EndDate,
                IsCompleted = project.IsCompleted,
                IsPublished = project.IsPublished,
                ImagePath = project.ImagePath,
                RecentProjects = recentProjects
            };

            return View(viewModel);
        }


        // GET: Projects/Create
        public IActionResult Create() => View();

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadFile(model.ImageFile);

                // Sanitize the content
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(model.Description);
                string plainTextContents = htmlDoc.DocumentNode.InnerText;

                var project = new Project
                {
                    Name = model.Name,
                    Description = plainTextContents,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsCompleted = model.IsCompleted,
                    IsPublished = model.IsPublished,
                    ImagePath = uniqueFileName
                };

                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            var viewModel = new ProjectEditViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = (DateTime)project.EndDate,
                IsCompleted = project.IsCompleted,
                IsPublished = project.IsPublished,
                ExistingImagePath = project.ImagePath
            };

            return View(viewModel);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                // Sanitize the content
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(model.Description);
                string plainTextContents = htmlDoc.DocumentNode.InnerText;

                var project = await _context.Projects.FindAsync(id);
                project.Name = model.Name;
                project.Description = plainTextContents;
                project.StartDate = model.StartDate;
                project.EndDate = model.EndDate;
                project.IsCompleted = model.IsCompleted;
                project.IsPublished = model.IsPublished;

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
                    project.ImagePath = UploadFile(model.ImageFile);
                }

                _context.Update(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            if (!string.IsNullOrEmpty(project.ImagePath))
            {
                string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", project.ImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Projects.Remove(project);
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
