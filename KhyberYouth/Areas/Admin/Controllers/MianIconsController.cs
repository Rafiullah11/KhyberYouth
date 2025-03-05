using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace KhyberYouth.Areas.Admin.Controllers
{
    [Authorize] // Restricts access to authenticated users
    [Area("Admin")]
    public class MainIconsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MainIconsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MainIcons
        public async Task<IActionResult> Index()
        {
            var iconList = await _context.MainIcons.ToListAsync();
            return View(iconList);
        }

        // GET: MainIcons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainIcon = await _context.MainIcons.FindAsync(id);
            if (mainIcon == null)
            {
                return NotFound();
            }

            var detailsViewModel = new MainIconDetailsViewModel
            {
                Id = mainIcon.Id,
                Title = mainIcon.Title,
                Description = mainIcon.Description,
                Icons = mainIcon.Icons,
                ActionName = mainIcon.ActionName,
                ControllerName = mainIcon.ControllerName,
                // Add any other properties that are specific to the Details view
            };

            return View(detailsViewModel);
        }

        // GET: MainIcons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MainIcons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MainIconCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var icon = new MainIcons
                {
                    Description = model.Description,
                    Title = model.Title,
                    Icons = model.Icons,
                    ActionName = model.ActionName,
                    ControllerName = model.ControllerName,
                };
                _context.Add(icon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: MainIcons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainIcon = await _context.MainIcons.FindAsync(id);
            if (mainIcon == null)
            {
                return NotFound();
            }

            var editViewModel = new MainIconEditViewModel
            {
                Id = mainIcon.Id,
                Title = mainIcon.Title,
                Description = mainIcon.Description,
                Icons = mainIcon.Icons,
                ControllerName = mainIcon.ControllerName,
                ActionName = mainIcon.ActionName,
            };

            return View(editViewModel);
        }

        // POST: MainIcons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MainIconEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var icon = await _context.MainIcons.FindAsync(id);
                    if (icon == null)
                    {
                        return NotFound();
                    }

                    icon.Title = model.Title;
                    icon.Description = model.Description;
                    icon.Icons = model.Icons;
                    icon.ControllerName = model.ControllerName;
                    icon.ActionName = model.ActionName;

                    _context.Update(icon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MainIconExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: MainIcons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainIcon = await _context.MainIcons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mainIcon == null)
            {
                return NotFound();
            }

            return View(mainIcon);
        }

        // POST: MainIcons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mainIcon = await _context.MainIcons.FindAsync(id);
            if (mainIcon != null)
            {
                _context.MainIcons.Remove(mainIcon);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MainIconExists(int id)
        {
            return _context.MainIcons.Any(e => e.Id == id);
        }
    }
}
