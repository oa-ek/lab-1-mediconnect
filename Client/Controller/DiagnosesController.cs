using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models;

namespace Client.Controllers
{
    public class DiagnosesController : Controller
    {
        private readonly Context _context;

        public DiagnosesController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var context = _context.Diagnoses.Include(d => d.Appointment).Include(d => d.Result);
            return View(await context.ToListAsync());
        }

        public async Task<IActionResult> Details(string? description)
        {
            if (description == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.Appointment)
                .Include(d => d.Result)
                .FirstOrDefaultAsync(m => m.Description == description);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        public IActionResult Create(int id)
        {
            ViewData["AppointmentId"] = id;
            ViewData["ResultId"] = new SelectList(_context.Results, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ResultId,Description,Date,AppointmentId")]
            Diagnosis diagnosis)
        {
            diagnosis.Date = DateTime.Now;
            await _context.AddAsync(diagnosis);
            await _context.SaveChangesAsync();
            if (diagnosis.AppointmentId > 0)
                return Redirect($"/Appointments/Details/{diagnosis.AppointmentId}");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "Id", "Id", diagnosis.AppointmentId);
            ViewData["ResultId"] = new SelectList(_context.Results, "Id", "Id", diagnosis.ResultId);
            return View(diagnosis);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,ResultId,Description,Date,AppointmentId")]
            Diagnosis diagnosis)
        {
            if (id != diagnosis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnosis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosisExists(diagnosis.Id))
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

            ViewData["AppointmentId"] = new SelectList(_context.Appointments, "Id", "Id", diagnosis.AppointmentId);
            ViewData["ResultId"] = new SelectList(_context.Results, "Id", "Id", diagnosis.ResultId);
            return View(diagnosis);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.Appointment)
                .Include(d => d.Result)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis is null) return NotFound();

            _context.Diagnoses.Remove(diagnosis);
            await _context.SaveChangesAsync();

            return Redirect($"/Appointments/Details/{diagnosis.AppointmentId}");
        }

        private bool DiagnosisExists(int id)
        {
            return _context.Diagnoses.Any(e => e.Id == id);
        }
    }
}