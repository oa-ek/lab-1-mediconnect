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
    public class DiagnosesController : Controller
    {
        private readonly Context _context;

        public DiagnosesController(Context context)
        {
            _context = context;
        }

        // GET: Diagnoses
        public async Task<IActionResult> Index()
        {
            var clinicContext = _context.Diagnoses.Include(d => d.Appointment).Include(d => d.Result);
            return View(await clinicContext.ToListAsync());
        }

        // GET: Diagnoses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.Appointment)
                .Include(d => d.Result)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // GET: Diagnoses/Create
        public IActionResult Create(int? id)
        {
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "ID", "ID", id);
            ViewData["ResultID"] = new SelectList(_context.Results, "ID", "Name");
            return View();
        }

        // POST: Diagnoses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ResultID,Description,Date,AppointmentID")] Diagnosis diagnosis)
        {
            Diagnosis temp = new Diagnosis { ResultID = diagnosis.ResultID, Description = diagnosis.Description, Date = DateTime.Now, AppointmentID = diagnosis.AppointmentID };
            _context.Add(temp);
            await _context.SaveChangesAsync();

            if(_context.Results.First(x =>x.ID == diagnosis.ResultID).Name.Equals("ПОТРЕБУЄ ПОВТОРНОГО ОБСТЕЖЕННЯ"))
            { 
                return Redirect("/Appointments/Create/"+ _context.Appointments.First(x => x.ID == diagnosis.AppointmentID).ClientID);
            }
            else
                return Redirect("/");
        }

        // GET: Diagnoses/Edit/5
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
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "ID", "ID", diagnosis.AppointmentID);
            ViewData["ResultID"] = new SelectList(_context.Results, "ID", "ID", diagnosis.ResultID);
            return View(diagnosis);
        }

        // POST: Diagnoses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ResultID,Description,Date,AppointmentID")] Diagnosis diagnosis)
        {
            if (id != diagnosis.ID)
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
                    if (!DiagnosisExists(diagnosis.ID))
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
            ViewData["AppointmentID"] = new SelectList(_context.Appointments, "ID", "ID", diagnosis.AppointmentID);
            ViewData["ResultID"] = new SelectList(_context.Results, "ID", "ID", diagnosis.ResultID);
            return View(diagnosis);
        }

        // GET: Diagnoses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.Appointment)
                .Include(d => d.Result)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // POST: Diagnoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diagnosis = await _context.Diagnoses.FindAsync(id);
            _context.Diagnoses.Remove(diagnosis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosisExists(int id)
        {
            return _context.Diagnoses.Any(e => e.ID == id);
        }
    }
}
