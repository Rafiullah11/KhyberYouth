using KhyberYouth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KhyberYouth.Components
{
    public class EventViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public EventViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var upcomingEvent = await _context.Events
                                              .AsNoTracking()
                                              .OrderBy(e => e.StartDate)
                                              .FirstOrDefaultAsync();
            return View(upcomingEvent);
        }
    }
}
