using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KhyberYouth.Components
{
    public class BannerViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public BannerViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var banners = _context.Banners.ToList(); // Assuming you have a DbSet<teamMember> in your context

            // Map to ViewModel
            var bannerViewModels = banners.Select(bannerVM => new BannerDetailsViewModel
            {
                ImagePath = bannerVM.ImagePath,
            }).ToList();

            return View(bannerViewModels);
        }
    }
}
