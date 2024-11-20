using KhyberYouth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhyberYouth.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            var post = await _context.BlogPosts.Include(p => p.BlogAuthor)
                                               .Include(p => p.BlogCategory).ToListAsync();  
            return View(post);
        }
    }
}
