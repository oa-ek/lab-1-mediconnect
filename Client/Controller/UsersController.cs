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
    public class UsersController : Controller
    {
        private readonly Context _context;
        private readonly FileManager _fileManager;

        public UsersController(Context context, FileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var context = _context.Users.Include(u => u.Gender).Include(u => u.Role).Include(u => u.Proffesion);
            return View(await context.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string? name)
        {
            if (name == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Gender)
                .Include(u => u.Role)
                .Include(u => u.Proffesion)
                .FirstOrDefaultAsync(m => m.FirstName == name);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["GenderID"] = new SelectList(_context.Genders, "ID", "ID");
            ViewData["RoleID"] = new SelectList(_context.Roles, "ID", "ID");
            ViewData["ProffesionID"] = new SelectList(_context.Proffesions, "ID", "ID");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AvatarFile,Login,Password,FirstName,SecondName,LastName,BirthDate,Phone,GenderID,RoleID,ProffesionID")] User user)
        {
            if (ModelState.IsValid)
            {
                // Обробка завантаження файлу
                if (user.AvatarFile != null)
                {
                    user.Avatar = await _fileManager.SaveUserPhotoAsync(user.AvatarFile);
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["GenderID"] = new SelectList(_context.Genders, "ID", "ID", user.GenderID);
            ViewData["RoleID"] = new SelectList(_context.Roles, "ID", "ID", user.RoleID);
            ViewData["ProffesionID"] = new SelectList(_context.Proffesions, "ID", "ID", user.ProffesionID);
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
            ViewData["GenderID"] = new SelectList(_context.Genders, "ID", "ID", user.GenderID);
            ViewData["RoleID"] = new SelectList(_context.Roles, "ID", "ID", user.RoleID);
            ViewData["ProffesionID"] = new SelectList(_context.Proffesions, "ID", "ID", user.ProffesionID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Avatar,Login,Password,FirstName,SecondName,LastName,BitrhDate,Phone,GenderID,RoleID,ProffesionID")] User user)
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
            ViewData["GenderID"] = new SelectList(_context.Genders, "ID", "ID", user.GenderID);
            ViewData["RoleID"] = new SelectList(_context.Roles, "ID", "ID", user.RoleID);
            ViewData["ProffesionID"] = new SelectList(_context.Proffesions, "ID", "ID", user.ProffesionID);
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
                .Include(u => u.Proffesion)
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
