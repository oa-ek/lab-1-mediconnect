using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models;

namespace MediConnect.Controllers
{
    public class UsersController : Controller
    {
        private readonly Context _context;

        public UsersController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var clinicContext = _context.Users.Include(u => u.Gender).Include(u => u.Role);
            return View(await clinicContext.ToListAsync());
        }

        public async Task<IActionResult> Clients()
        {
            var clinicContext = _context.Users.Include(u => u.Gender).Include(u => u.Role)
                .Where(x => x.Role.Name.Equals("Patient"));
            return View("Index", await clinicContext.ToListAsync());
        }

        public async Task<IActionResult> MyClients()
        {
            var currentUser = _context.Users.First(x => x.Login.Equals(HttpContext.User.Identity.Name));
            var clientsIds = _context.Appointments.Where(x => x.DoctorId == currentUser.Id).Select(x => x.ClientId)
                .Distinct();
            var clinicContext = _context.Users.Include(u => u.Gender).Include(u => u.Role)
                .Where(x => x.Role.Name.Equals("Patient") && clientsIds.Contains(x.Id));
            return View("Index", await clinicContext.ToListAsync());
        }

        public async Task<IActionResult> Doctors()
        {
            var clinicContext = _context.Users.Include(u => u.Gender).Include(u => u.Role)
                .Where(x => !x.Role.Name.Equals("Patient"));
            return View("Index", await clinicContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            var dateTime = DateTime.ParseExact("01.01.0001 00:00:00", "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            ViewBag.End = dateTime;

            if (id == null)
            {
                ViewBag.My = true;
                var currentUser = _context.Users.Include(x => x.Role).Include(x => x.Gender).Include(x => x.Reviews)
                    .First(x => x.Login.Equals(HttpContext.User.Identity.Name));
                ViewBag.Appointments = _context.Appointments.Include(x => x.Doctor).Include(x => x.Client)
                    .Where(x => x.ClientId == currentUser.Id || x.DoctorId == currentUser.Id);
                ViewBag.Reviews = _context.Reviews.Include(x => x.Doctor).Include(x => x.Client)
                    .Where(x => x.DoctorId == currentUser.Id);
                return View(currentUser);
            }

            ViewBag.Appointments = _context.Appointments.Include(x => x.Doctor).Include(x => x.Client)
                .Where(x => x.ClientId == id || x.DoctorId == id);
            ViewBag.Reviews = _context.Reviews.Include(x => x.Doctor).Include(x => x.Client)
                .Where(x => x.DoctorId == id);
            ViewBag.My = false;
            var user = await _context.Users
                .Include(u => u.Gender)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Create()
        {
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name");
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(x => x.Id != 82), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Avatar,Login,Password,FirstName,SecondName,LastName,BirthDate,Phone,GenderId,RoleId")]
            User user)
        {
            var avatar = user.GenderId == 1 ? "/img/men_avatar.png" : "/img/women_avatar.png";
            user.Avatar = avatar;
            user.Role = (await _context.Roles.FindAsync(user.RoleId))!;

            await _context.AddAsync(user);
            var result = await _context.SaveChangesAsync() > 0;

            if (result)
            {
                if (_context.Roles.First(x => x.Id == user.RoleId).Name.Equals("Patient"))
                    return Redirect("Clients/");
                return Redirect("Doctors/");
            }

            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Id", user.GenderId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", user.RoleId);
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", user.GenderId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Avatar,Login,Password,FirstName,SecondName,LastName,BirthDate,Phone,GenderId,RoleId")]
            User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            _context.Update(user);
            var result = await _context.SaveChangesAsync() > 0;

            if (result)
                return RedirectToAction(nameof(Index));

            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", user.GenderId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Gender)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}