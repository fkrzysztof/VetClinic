using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.PortalWWW.Controllers
{
    public class LoginController : Controller
    {
        private readonly VetClinicContext _context;

        public LoginController(VetClinicContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
            User account = _context.Users.First(u => u.Login == user.Login);

            if (account != null && account.Password == user.Password)
            {
                HttpContext.Session.SetString("UserID", account.UserID.ToString());
                HttpContext.Session.SetString("Login", account.Login.ToString());

                return RedirectToAction("Index", "ClientPanel");
            }
            else
            {
                ModelState.AddModelError("", "Login lub hasło jest niepoprawne.");
            }

            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                ModelState.Clear();
                ViewBag.Message = user.FirstName + " " + user.LastName + " pomyślnie zarejestrowano konto.";
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}