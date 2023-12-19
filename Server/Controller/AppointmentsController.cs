using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models;

namespace Server.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly Context _context;

        public AppointmentsController(Context context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            DateTime dateTime = DateTime.Parse("01.01.0001 0:00:00");
            ViewBag.End = dateTime;
            var currentUser = _context.Users.Include(x => x.Role).First(x => x.Login.Equals(HttpContext.User.Identity.Name));
            var clinicContext = _context.Appointments.Include(a => a.Client).Include(a => a.Doctor).Where(x => x.Doctor.ID == currentUser.ID).OrderByDescending(x => x.StartDate);
            return View(await clinicContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DateTime dateTime = DateTime.Parse("01.01.0001 0:00:00");
            ViewBag.End = dateTime;

            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Doctor)
                .Include(a => a.Diagnosises)
                .Include(a => a.Discussions)
                .FirstOrDefaultAsync(m => m.ID == id);

            List<Diagnosis> list = new List<Diagnosis>();

            foreach (Diagnosis diagnosis in appointment.Diagnosises)
            {
                list.Add(_context.Diagnoses.Include(x => x.Result).First(x => x.ID == diagnosis.ID));
            }

            appointment.Diagnosises = list;

            ViewBag.Discussions = _context.Discussions.Include(x => x.Doctor).Where(x => x.AppointmentID == id);


            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Finish
        public IActionResult Finish(int? id)
        {
            var appointment = _context.Appointments.First(x => x.ID == id);
            appointment.EndDate = DateTime.Now;
            _context.Update(appointment);
            _context.SaveChanges();
            return Redirect("/Diagnoses/Create/" + id);
        }

        // GET: Appointments/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                ViewBag.My = true;
                var currentUser = _context.Users.Include(x => x.Role).First(x => x.Login.Equals(HttpContext.User.Identity.Name));
                ViewData["ClientID"] = new SelectList(_context.Users.Include(x => x.Role).Where(x => x.Role.Name.Equals("Patient")), "ID", "FullName", currentUser.ID);
            }
            else
            {
                ViewData["ClientID"] = new SelectList(_context.Users.Include(x => x.Role).Where(x => x.Role.Name.Equals("Patient")), "ID", "FullName", id);
            }
            var doctor = _context.Users.Include(x => x.Role).First(x => x.Login.Equals(HttpContext.User.Identity.Name));
            if (!doctor.Role.Name.Equals("Patient") || !doctor.Role.Name.Equals("Admin") || !doctor.Role.Name.Equals("Clinic Manager"))
                ViewData["DoctorID"] = new SelectList(_context.Users.Include(x => x.Role).Where(x => !x.Role.Name.Equals("Patient")), "ID", "FullName", doctor.ID);
            else
                ViewData["DoctorID"] = new SelectList(_context.Users.Include(x => x.Role).Where(x => !x.Role.Name.Equals("Patient")), "ID", "FullName");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ClientID,DoctorID,StartDate")] Appointment appointment)
        {
            Appointment appointment1 = new Appointment { ClientID = appointment.ClientID, DoctorID = appointment.DoctorID, StartDate = appointment.StartDate };
            _context.Add(appointment1);
            await _context.SaveChangesAsync();
            return Redirect("/");
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["ClientID"] = new SelectList(_context.Users, "ID", "ID", appointment.ClientID);
            ViewData["DoctorID"] = new SelectList(_context.Users, "ID", "ID", appointment.DoctorID);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClientID,DoctorID,StartDate,EndDate")] Appointment appointment)
        {
            if (id != appointment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.ID))
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
            ViewData["ClientID"] = new SelectList(_context.Users, "ID", "ID", appointment.ClientID);
            ViewData["DoctorID"] = new SelectList(_context.Users, "ID", "ID", appointment.DoctorID);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.ID == id);
        }
    }
}
