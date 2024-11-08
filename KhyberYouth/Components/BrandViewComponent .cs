using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KhyberYouth.Components
{
    public class BrandViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public BrandViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var brands = _context.Brands.ToList(); // Assuming you have a DbSet<teamMember> in your context

            // Map to ViewModel
            var brandViewModels = brands.Select(brandVM => new BrandDetailsViewModel
            {
                ImagePath = brandVM.ImagePath,
            }).ToList();

            return View(brandViewModels);
        }
    }
}
