using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace KhyberYouth.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contact
        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

        // POST: Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Message = model.Message
                };

                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thank you for reaching out! We will get back to you soon.";
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}