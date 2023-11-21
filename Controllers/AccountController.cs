using MediConnect.Data;
using MediConnect.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MediConnect.Controllers
{
    public class AccountController : Controller
    {
        private Context db;
        public AccountController(Context context)
        {
            db = context;
        }
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
                User user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) Password");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Genders = new SelectList(db.Genders, "ID", "Name");
            ViewBag.Roles = new SelectList(db.Roles, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    //TODO ADD DEFAULT VALUE FOR ROLE ID CLIENT --- NOTE REGISTER ONLY FOR CLIENTS...

                    string avatar = model.GenderID == 1 ? "/img/men_avatar.png" : "/img/women_avatar.png";
                    User registerUser = new User {
                        FirstName = model.FirstName,
                        SecondName = model.SecondName,
                        LastName = model.LastName,
                        Phone = model.Phone,
                        Avatar = avatar,
                        Login = model.Login,
                        Password = model.Password,
                        GenderID = model.GenderID,
                        RoleID = 82,
                        BitrhDate = model.BirthDate
                    };

                    // добавляем пользователя в бд
                    db.Users.Add(registerUser);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Login", "Account");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) Password");
            }
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
