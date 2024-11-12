using KhyberYouth.Models;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KhyberYouth.Components
{
    public class BankDetailsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public BankDetailsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Fetch bank details from the database
            var bankDetail = _context.BankAccountDetails.FirstOrDefault();

            return View(bankDetail); // Pass the model directly to the view
            
        }
    }
}