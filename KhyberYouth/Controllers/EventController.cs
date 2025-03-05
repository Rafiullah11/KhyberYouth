using KhyberYouth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KhyberYouth.Controllers
{
    [AllowAnonymous]
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
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

       

    }
}
