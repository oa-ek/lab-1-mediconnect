using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public DiscussionsController(Context context)
        {
            _context = context;
        }

        // GET: Discussions
        public async Task<IActionResult> Index()
        {
            var context = _context.Discussions.Include(d => d.Appointment).Include(d => d.Doctor);
            return View(await context.ToListAsync());
        }

        // GET: Discussions/Details/5
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

        // GET: Discussions/Create
        public IActionResult Create()
        {
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "ID", "ID");
            ViewData["DoctorID"] = new SelectList(_context.Users, "ID", "ID");
            ViewData["ProffesionID"] = new SelectList(_context.Proffesions, "ID", "ID");
            return View();
        }

        // POST: Discussions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AppointmentID,DoctorID,Rate,MessageDate,Message")] Discussion discussion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "ID", "ID", discussion.AppointmentID);
            ViewData["DoctorID"] = new SelectList(_context.Users, "ID", "ID", discussion.DoctorID);
            return View(discussion);
        }

        // GET: Discussions/Edit/5
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
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "ID", "ID", discussion.AppointmentID);
            ViewData["DoctorID"] = new SelectList(_context.Users, "ID", "ID", discussion.DoctorID);
            return View(discussion);
        }

        // POST: Discussions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AppointmentID,DoctorID,Rate,MessageDate,Message")] Discussion discussion)
        {
            if (id != discussion.ID)
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
                    if (!DiscussionExists(discussion.ID))
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
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "ID", "ID", discussion.AppointmentID);
            ViewData["DoctorID"] = new SelectList(_context.Users, "ID", "ID", discussion.DoctorID);
            return View(discussion);
        }

        // GET: Discussions/Delete/5
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

        // POST: Discussions/Delete/5
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
            return _context.Discussions.Any(e => e.ID == id);
        }
    }
}
