using System;
using System.Linq;
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
            return View(_context.Users.ToList());
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var account = _context.Users.Where(u => u.Login == user.Login && u.Password == user.Password).FirstOrDefault();
            if (account != null)
            {
                HttpContext.Session.SetString("UserID", account.UserID.ToString());
                HttpContext.Session.SetString("Login", account.Login.ToString());
                return RedirectToAction("Welcome");
            }
            else
            {
                ModelState.AddModelError("","Login lub hasło jest niepoprawne.");
            }
            return View();
        }

        public IActionResult Welcome()
        {
           if(HttpContext.Session.GetString("Login") != null)
           {
               ViewBag.Login = HttpContext.Session.GetString("Login");
               return View();
           }
           else
           {
               return RedirectToAction("Login");
           }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}