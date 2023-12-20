using Server.Data;
using Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Server.Controllers
{
    public class AccountController(Context context) : Controller
    {
        private Context _db = context;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) Password");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Genders = new SelectList(_db.Genders, "Id", "Name");
            ViewBag.Roles = new SelectList(_db.Roles, "Id", "Name");
            // ViewBag.Professions = new SelectList(_db.Proffesions, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
            if (user == null)
            {
                // var profession = await _db.Proffesions.FindAsync(model.ProfessionId);
                // if (profession == null)
                // {
                //     ModelState.AddModelError("ProfessionID", "Вибрана професія не існує.");
                //     // ViewBag.Professions = new SelectList(_db.Proffesions, "ID", "Name");
                //     return View(model);
                // }

                var avatar = model.GenderId == 1 ? "/img/men_avatar.png" : "/img/women_avatar.png";
                var registerUser = new User
                {
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    Avatar = avatar,
                    Login = model.Login,
                    Password = model.Password,
                    GenderId = model.GenderId,
                    RoleId = 2,
                    BirthDate = model.BirthDate,
                    ProffesionId = model.ProfessionId
                };

                _db.Users.Add(registerUser);
                await _db.SaveChangesAsync();
                return RedirectToAction("Login", "Account");
            }
            else
                ModelState.AddModelError("", "Некорректные логин и(или) Password");

            // ViewBag.Professions = new SelectList(_db.Proffesions, "ID", "Name");
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
