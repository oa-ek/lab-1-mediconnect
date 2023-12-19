using Client.Services.EmailSender;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models;

namespace Client.Controllers
{
    public class DiscussionsController : Controller
    {
        private readonly Context _context;
        private readonly IEmailSender _emailSender;

        public DiscussionsController(Context context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var context = _context.Discussions.Include(d => d.Appointment).Include(d => d.Doctor);
            return View(await context.ToListAsync());
        }

        public async Task<IActionResult> MessageSend(int appointmentId, string message)
        {
            var appointment = await _context.Appointments.Include(x => x.Doctor).Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id == appointmentId);

            if (appointment!.Client.Email is not null)
            {
                await _emailSender.SendEmailAsync(appointment.Client.Email,
                    $"Appointment at Doctor {appointment.Doctor.FullName}", message);
                return Redirect($"/Appointments/Details/{appointment.Id}");
            }

            return BadRequest("Couldn't send email. User probably doesn't have one");
        }

        public async Task<IActionResult> Details(string? message)
        {
            if (message == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions
                .Include(d => d.Appointment)
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(m => m.Message == message);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        public IActionResult Create()
        {
            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "Id", "Id");
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,AppointmentId,DoctorId,Rate,MessageDate,Message")]
            Discussion discussion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "Id", "Id", discussion.AppointmentId);
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id", discussion.DoctorId);
            return View(discussion);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion == null)
            {
                return NotFound();
            }

            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "Id", "Id", discussion.AppointmentId);
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id", discussion.DoctorId);
            return View(discussion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,AppointmentId,DoctorId,Rate,MessageDate,Message")]
            Discussion discussion)
        {
            if (id != discussion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionExists(discussion.Id))
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

            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "Id", "Id", discussion.AppointmentId);
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id", discussion.DoctorId);
            return View(discussion);
        }

        public async Task<IActionResult> Delete(string? message)
        {
            if (message == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions
                .Include(d => d.Appointment)
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(m => m.Message == message);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string message)
        {
            var discussion = await _context.Discussions.FindAsync(message);
            _context.Discussions.Remove(discussion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussions.Any(e => e.Id == id);
        }
    }
}