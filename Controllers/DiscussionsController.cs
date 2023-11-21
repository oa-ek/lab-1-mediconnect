using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediConnect.Data;
using MediConnect.Models;

namespace MediConnect.Controllers
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

        // GET: Raitings/Rate
        public IActionResult MessageSend(int? AppointmentID, string? Message)
        {
            var _currentUser = _context.Users.First(x => x.Login.Equals(HttpContext.User.Identity.Name));
            Discussion discussion = new Discussion { AppointmentID = (int)AppointmentID, DoctorID = (int)(_currentUser.ID), Rate = 0, MessageDate = DateTime.Now, Message = Message};
            _context.Add(discussion);
            _context.SaveChanges();

            return Redirect("~/Appointments/Details/" + AppointmentID);
        }


        // GET: Discussions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions
                .Include(d => d.Appointment)
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(m => m.ID == id);
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussions
                .Include(d => d.Appointment)
                .Include(d => d.Doctor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussions.FindAsync(id);
            _context.Discussions.Remove(discussion);
            await _context.SaveChangesAsync();
            return Redirect("/Appointments/Details/" + discussion.AppointmentID);
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussions.Any(e => e.ID == id);
        }
    }
}
