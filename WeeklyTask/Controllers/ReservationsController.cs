using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeeklyTask.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace WeeklyTask.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly FoodDbContext _context;
        private readonly IConfiguration _configuration;

        public ReservationsController(FoodDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Reservations1
        public async Task<IActionResult> Index()
        {
              return _context.Reservations != null ? 
                          View(await _context.Reservations.ToListAsync()) :
                          Problem("Entity set 'FoodDbContext.Reservations'  is null.");
        }

        // GET: Reservations1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*public async Task<IActionResult> Create([Bind("Id,ReservationDate,PartySize,Notes,ContactName,ContactPhone")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();

                // Send the email notification
                var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
                var fromAddress = new MailAddress(smtpSettings.Username, "Yummy Restaurant");
                var toAddress = new MailAddress("nelson.chasemedia@gmail.com", "Nelson");
                const string subject = "New Reservation";
                const string body = "A new reservation has been created.";
                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                };
                using (var smtpClient = new SmtpClient(smtpSettings.Server, smtpSettings.Port))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(message);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }*/
    

        public async Task<IActionResult> Create([Bind("Id,ReservationDate,PartySize,Notes,ContactName,ContactPhone")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();

                // Send the email notification
                var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
                var fromAddress = new MailAddress(smtpSettings.Username, "Yummy Restaurant");
                var toAddress = new MailAddress("nelson.chasemedia@gmail.com", "Nelson");
                const string subject = "New Reservation";
                const string body = "A new reservation has been created.";
                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                };
                using (var smtpClient = new SmtpClient(smtpSettings.Server, smtpSettings.Port))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(message);
                }

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }



        // GET: Reservations1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReservationDate,PartySize,Notes,ContactName,ContactPhone")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            return View(reservation);
        }

        // GET: Reservations1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'FoodDbContext.Reservations'  is null.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
          return (_context.Reservations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
