using Microsoft.AspNetCore.Mvc;
using KhyberYouth.Models; 
using KhyberYouth.ViewModel;

namespace KhyberYouth.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context; 

        public BlogViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var blogs = _context.BlogPosts.ToList(); 

            // Map to ViewModel
            var blogsViewModels = blogs.Select(blog => new BlogDetailsViewModel
            {
               Id = blog.Id,
               Title = blog.Title,
               Contents = blog.Contents,
               PublishedOn = blog.PublishedOn,
               IsPublish = blog.IsPublish,
               ImagePath = blog.ImagePath
            }).ToList();

            return View(blogsViewModels); // Pass ViewModel to the view
        }
    }
}
