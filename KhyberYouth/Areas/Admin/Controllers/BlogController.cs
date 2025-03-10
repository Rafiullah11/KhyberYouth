﻿using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhyberYouth.Areas.Admin.Controllers
{
    [Authorize] // Restricts access to authenticated users
    [Area("Admin")]
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

        // GET: BlogPosts
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var blogs = _context.BlogPosts.AsNoTracking();
            int pageSize = 4;
            return View(await PaginatedList<BlogPost>.CreateAsync(blogs, pageNumber ?? 1, pageSize));
        }

        // GET: BlogPosts/Create
        public IActionResult Create() => View();

        // POST: BlogPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadFile(model.ImageFile);
                var blogPost = new BlogPost
                {
                    Title = model.Title,
                    Contents = model.Contents,
                    PublishedOn = DateTime.UtcNow,
                    IsPublish = model.IsPublish,
                    ImagePath = uniqueFileName
                };
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null) return NotFound();

            var viewModel = new BlogEditViewModel
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Contents = blogPost.Contents,
                IsPublish = blogPost.IsPublish,
                ExistImage = blogPost.ImagePath
            };

            return View(viewModel);
        }

        // POST: BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var blogPost = await _context.BlogPosts.FindAsync(id);
                blogPost.Title = model.Title;
                blogPost.Contents = model.Contents;
                blogPost.IsPublish = model.IsPublish;

                if (model.ImageFile != null)
                {
                    if (!string.IsNullOrEmpty(model.ExistImage))
                    {
                        string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", model.ExistImage);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    blogPost.ImagePath = UploadFile(model.ImageFile);
                }

                _context.Update(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null) return NotFound();
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null) return NotFound();

            if (!string.IsNullOrEmpty(blogPost.ImagePath))
            {
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", blogPost.ImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private string UploadFile(IFormFile imageFile)
        {
            string uniqueFileName = null;
            if (imageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
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