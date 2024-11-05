using Microsoft.AspNetCore.Mvc;

namespace KhyberYouth.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
