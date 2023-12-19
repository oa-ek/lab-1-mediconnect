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
    public class UsersController : Controller
    {
        private readonly Context _context;

        public UsersController(Context context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var clinicContext = _context.Users.Include(u => u.Gender).Include(u => u.Role);
            return View(await clinicContext.ToListAsync());
        }

        // GET: Clients
        public async Task<IActionResult> Clients()
        {
            var clinicContext = _context.Users.Include(u => u.Gender).Include(u => u.Role).Where(x=>x.Role.Name.Equals("Patient"));
            return View("Index", await clinicContext.ToListAsync());
        }

        // GET: MyClients
        public async Task<IActionResult> MyClients()
        {

            var currentUser = _context.Users.First(x => x.Login.Equals(HttpContext.User.Identity.Name));
            var clientsIDS = _context.Appointments.Where(x => x.DoctorID == currentUser.ID).Select(x => x.ClientID).Distinct();
            var clinicContext = _context.Users.Include(u => u.Gender).Include(u => u.Role).Where(x => x.Role.Name.Equals("Patient") && clientsIDS.Contains(x.ID));
            return View("Index", await clinicContext.ToListAsync());
        }

        // GET: Doctors
        public async Task<IActionResult> Doctors()
        {
            var clinicContext = _context.Users.Include(u => u.Gender).Include(u => u.Role).Where(x => !x.Role.Name.Equals("Patient"));
            return View("Index", await clinicContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            DateTime dateTime = DateTime.Parse("01.01.0001 0:00:00");
            ViewBag.End = dateTime;

            if (id == null)
            {

                ViewBag.My = true;
                var currentUser = _context.Users.Include(x => x.Role).Include(x=>x.Gender).Include(x=>x.Reviews).First(x => x.Login.Equals(HttpContext.User.Identity.Name));
                ViewBag.Appointments = _context.Appointments.Include(x=>x.Doctor).Include(x=>x.Client).Where(x=>x.ClientID == currentUser.ID || x.DoctorID == currentUser.ID);
                ViewBag.Reviews = _context.Reviews.Include(x => x.Doctor).Include(x => x.Client).Where(x => x.DoctorID == currentUser.ID);
                return View(currentUser);
            }

            ViewBag.Appointments = _context.Appointments.Include(x => x.Doctor).Include(x => x.Client).Where(x => x.ClientID == id|| x.DoctorID == id);
            ViewBag.Reviews = _context.Reviews.Include(x => x.Doctor).Include(x => x.Client).Where(x => x.DoctorID == id);
            ViewBag.My = false;
            var user = await _context.Users
                .Include(u => u.Gender)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["GenderID"] = new SelectList(_context.Genders, "ID", "Name");
            ViewData["RoleID"] = new SelectList(_context.Roles.Where(x=>x.ID!=82), "ID", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Avatar,Login,Password,FirstName,SecondName,LastName,BitrhDate,Phone,GenderID,RoleID")] User user)
        {
            if (ModelState.IsValid)
            {
                string avatar = user.GenderID == 1 ? "/img/men_avatar.png" : "/img/women_avatar.png";
                user.Avatar = avatar;
                _context.Add(user);
                await _context.SaveChangesAsync();

                if (_context.Roles.First(x => x.ID == user.RoleID).Name.Equals("Patient"))
                    return Redirect("Clients/");
                return Redirect("Employees/");
            }
            ViewData["GenderID"] = new SelectList(_context.Genders, "ID", "ID", user.GenderID);
            ViewData["RoleID"] = new SelectList(_context.Roles, "ID", "ID", user.RoleID);
            return View(user);
        }

        // GET: Users/Edit/5
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
            ViewData["GenderID"] = new SelectList(_context.Genders, "ID", "Name", user.GenderID);
            ViewData["RoleID"] = new SelectList(_context.Roles, "ID", "Name", user.RoleID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Avatar,Login,Password,FirstName,SecondName,LastName,BitrhDate,Phone,GenderID,RoleID")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
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
            ViewData["GenderID"] = new SelectList(_context.Genders, "ID", "Name", user.GenderID);
            ViewData["RoleID"] = new SelectList(_context.Roles, "ID", "Name", user.RoleID);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Gender)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
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
            return _context.Users.Any(e => e.ID == id);
        }
    }
}
