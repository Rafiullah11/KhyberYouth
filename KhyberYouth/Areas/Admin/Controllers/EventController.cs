using KhyberYouth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KhyberYouth.Areas.Admin.Controllers
{
    [Authorize] // Restricts access to authenticated users
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Event
        public IActionResult Index()
        {
            var events = _context.Events.OrderBy(e => e.StartDate).ToList();
            return View(events);
        }

        // GET: Event/Details/5
        public IActionResult Details(int id)
        {
            var eventDetails = _context.Events.FirstOrDefault(e => e.Id == id);
            if (eventDetails == null)
            {
                return NotFound();
            }
            return View(eventDetails);
        }
        
        // GET: AllEvent/Details/5
        public IActionResult AllEvent()
        {
            var events = _context.Events.OrderBy(e => e.StartDate).ToList();
            return View(events);
        }
        // GET: Event
        //public IActionResult Index()
        //{
        //    var upcomingEvent = _context.Events.OrderBy(e => e.StartDate).FirstOrDefault();
        //    if (upcomingEvent == null)
        //    {
        //        return View("NoUpcomingEvent"); // Show a message if no event is found
        //    }

        //    return View(upcomingEvent);
        //}

        [HttpPost]
        public IActionResult Subscribe(int eventId, string email)
        {
            if (string.IsNullOrEmpty(email) || !new EmailAddressAttribute().IsValid(email))
            {
                return Json(new { success = false, message = "Please provide a valid email address." });
            }

            var subscription = new Subscription { EventId = eventId, Email = email };
            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();

            return Json(new { success = true, message = "Subscription successful!" });
        }
        public IActionResult GetUpcomingEvent()
        {
            var upcomingEvent = _context.Events.OrderBy(e => e.StartDate).FirstOrDefault();
            if (upcomingEvent == null)
            {
                return Json(null);
            }

            return Json(new
            {
                Title = upcomingEvent.Title,
                Location = upcomingEvent.Location,
                StartDate = upcomingEvent.StartDate
            });
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event newEvent)
        {
            if (ModelState.IsValid)
            {
                newEvent.CreatedAt = DateTime.Now;
                _context.Events.Add(newEvent);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(newEvent);
        }

        // GET: Event/Edit/5
        public IActionResult Edit(int id)
        {
            var eventToEdit = _context.Events.FirstOrDefault(e => e.Id == id);
            if (eventToEdit == null)
            {
                return NotFound();
            }
            return View(eventToEdit);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Event updatedEvent)
        {
            if (id != updatedEvent.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var existingEvent = _context.Events.FirstOrDefault(e => e.Id == id);
                if (existingEvent == null)
                {
                    return NotFound();
                }

                existingEvent.Title = updatedEvent.Title;
                existingEvent.Location = updatedEvent.Location;
                existingEvent.StartDate = updatedEvent.StartDate;
                existingEvent.EndDate = updatedEvent.EndDate;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(updatedEvent);
        }

        // GET: Event/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete == null)
            {
                return NotFound();
            }

            return View(eventToDelete);
        }

        // POST: Event/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Event model)
        {
            var eventToDelete = await _context.Events.FindAsync(id);
            if (eventToDelete == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
