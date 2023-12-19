using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models;

namespace Client.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly Context _context;

        public ReviewsController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var context = _context.Reviews.Include(r => r.Client).Include(r => r.Doctor);
            return View(await context.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Client)
                .Include(r => r.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,ClientId,DoctorId,Rate,ReviewDate,Description")]
            Review review)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Login == User.Identity!.Name);
            review.ClientId = currentUser!.Id;
            review.ReviewDate = DateTime.Now;

            _context.Add(review);

            var result = await _context.SaveChangesAsync() > 0;

            if (result)
                return RedirectToAction(nameof(Index));

            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", review.ClientId);
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id", review.DoctorId);
            return View(review);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", review.ClientId);
            ViewData["DoctorId"] = new SelectList(_context.Users, "Id", "Id", review.DoctorId);
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,ClientId,DoctorId,Rate,ReviewDate,Description")]
            Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }


            _context.Update(review);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Client)
                .Include(r => r.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string description)
        {
            var review = await _context.Reviews.FindAsync(description);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}