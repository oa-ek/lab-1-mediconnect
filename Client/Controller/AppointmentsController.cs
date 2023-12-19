using System.Globalization;
using Client.Services.EmailSender;
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
        private readonly IEmailSender _emailSender;

        public AppointmentsController(Context context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var dateTime = DateTime.ParseExact("01.01.0001 00:00:00", "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            ViewBag.End = dateTime;

            var currentUser = _context.Users.Include(x => x.Role)
                .First(x => x.Login.Equals(HttpContext.User.Identity.Name));

            var clinicContext = _context.Appointments.Include(a => a.Client)
                .Include(a => a.Doctor)
                .Where(x => x.Doctor.Id == currentUser.Id || x.ClientId == currentUser.Id)
                .OrderByDescending(x => x.StartDate);

            return View(await clinicContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dateTime = DateTime.ParseExact("01.01.0001 00:00:00", "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            ViewBag.End = dateTime;

            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Doctor)
                .Include(a => a.Diagnosises)
                .Include(a => a.Discussions)
                .FirstOrDefaultAsync(m => m.Id == id);

            List<Diagnosis> list = new();

            foreach (var diagnosis in appointment.Diagnosises)
            {
                list.Add(_context.Diagnoses.Include(x => x.Result).First(x => x.Id == diagnosis.Id));
            }

            appointment.Diagnosises = list;

            ViewBag.Discussions = _context.Discussions.Include(x => x.Doctor).Where(x => x.AppointmentId == id);


            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        public IActionResult Finish(int? id)
        {
            var appointment = _context.Appointments.First(x => x.Id == id);
            appointment.EndDate = DateTime.Now;
            _context.Update(appointment);
            _context.SaveChanges();
            return Redirect("/Diagnoses/Create/" + id);
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                ViewBag.My = true;
                var currentUser = _context.Users.Include(x => x.Role)
                    .First(x => x.Login.Equals(HttpContext.User.Identity.Name));
                ViewData["ClientId"] =
                    new SelectList(_context.Users.Include(x => x.Role).Where(x => x.Role.Name.Equals("Patient")), "Id",
                        "FullName", currentUser.Id);
            }
            else
            {
                ViewData["ClientId"] =
                    new SelectList(_context.Users.Include(x => x.Role).Where(x => x.Role.Name.Equals("Patient")), "Id",
                        "FullName", id);
            }

            var doctor = _context.Users.Include(x => x.Role).First(x => x.Login.Equals(HttpContext.User.Identity.Name));
            if (!doctor.Role.Name.Equals("Patient") || !doctor.Role.Name.Equals("Admin") ||
                !doctor.Role.Name.Equals("Clinic Manager"))
                ViewData["DoctorId"] =
                    new SelectList(_context.Users.Include(x => x.Role).Where(x => !x.Role.Name.Equals("Patient")), "Id",
                        "FullName", doctor.Id);
            else
                ViewData["DoctorId"] =
                    new SelectList(_context.Users.Include(x => x.Role).Where(x => !x.Role.Name.Equals("Patient")), "Id",
                        "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,DoctorId,StartDate")] Appointment appointment)
        {
            var appointment1 = new Appointment
                { ClientId = appointment.ClientId, DoctorId = appointment.DoctorId, StartDate = appointment.StartDate };

            _context.Add(appointment1);
            await _context.SaveChangesAsync();

            var doctor = await _context.Users.FindAsync(appointment.DoctorId);

            if (doctor is null)
                return BadRequest("Doctor can't be null");

            var client = await _context.Users.FindAsync(appointment.ClientId);

            if (client is not null && client.Email is not null)
                await _emailSender.SendEmailAsync(client.Email, $"Appointment to {doctor.FullName}",
                    $"You are appointed to {doctor.FullName} at {appointment.StartDate}");

            return Redirect("/");
        }

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

            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", appointment.ClientId);
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id", appointment.DoctorId);
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,ClientId,DoctorId,StartDate,EndDate")]
            Appointment appointment)
        {
            if (id != appointment.Id)
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
                    if (!AppointmentExists(appointment.Id))
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

            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", appointment.ClientId);
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id", appointment.DoctorId);
            return View(appointment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

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
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}