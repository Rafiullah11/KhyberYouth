using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KhyberYouth.Models;

namespace KhyberYouth.Controllers
{
    public class BankAccountDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BankAccountDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BankAccountDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.BankAccountDetails.ToListAsync());
        }

        // GET: BankAccountDetails/Details/5
        [HttpGet]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccountDetail = await _context.BankAccountDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bankAccountDetail == null)
            {
                return NotFound();
            }

            return View(bankAccountDetail);
        }

        // GET: BankAccountDetails/Create
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        // POST: BankAccountDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountTitle,AccountNumber,IbanNumber,CurrencyType,BankName,BranchCode,SwiftCode,BankAddress,ContactPhoneNumber")] BankAccountDetail bankAccountDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bankAccountDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bankAccountDetail);
        }

        // GET: BankAccountDetails/Edit/5
        [HttpGet]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccountDetail = await _context.BankAccountDetails.FindAsync(id);
            if (bankAccountDetail == null)
            {
                return NotFound();
            }
            return View(bankAccountDetail);
        }

        // POST: BankAccountDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountTitle,AccountNumber,IbanNumber,CurrencyType,BankName,BranchCode,SwiftCode,BankAddress,ContactPhoneNumber")] BankAccountDetail bankAccountDetail)
        {
            if (id != bankAccountDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bankAccountDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankAccountDetailExists(bankAccountDetail.Id))
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
            return View(bankAccountDetail);
        }

        // GET: BankAccountDetails/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccountDetail = await _context.BankAccountDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bankAccountDetail == null)
            {
                return NotFound();
            }

            return View(bankAccountDetail);
        }

        // POST: BankAccountDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bankAccountDetail = await _context.BankAccountDetails.FindAsync(id);
            if (bankAccountDetail != null)
            {
                _context.BankAccountDetails.Remove(bankAccountDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankAccountDetailExists(int id)
        {
            return _context.BankAccountDetails.Any(e => e.Id == id);
        }
    }
}
