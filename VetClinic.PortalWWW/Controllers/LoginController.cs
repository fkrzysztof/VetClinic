using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data;
using VetClinic.Data.Data.VetClinic;
using VetClinic.PortalWWW.Helpers;

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
            if(account.IsActive==false)
            {
                ModelState.AddModelError("", "Twoje konto jest zablokowane.");
                return View();
            }

            if (account != null && account.Password == user.Password)
            {
                HttpContext.Session.SetString("UserID", account.UserID.ToString());
                HttpContext.Session.SetString("Login", account.Login.ToString());

                return RedirectToAction("Index", "ClientPanel");
            }
            else
            {
                ModelState.AddModelError("", "Login lub hasło jest niepoprawne.");
                account.LoginAttempt++;
                _context.SaveChanges();
                if (account.LoginAttempt >= 5)
                {
                    account.IsActive = false;
                    _context.SaveChanges();
                    ModelState.AddModelError("", "Twoje konto zostalo zablokowane.");
                }
                          
                //ModelState.AddModelError("", "Login lub hasło jest niepoprawne.");
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
                string message = "Witaj " + user.FirstName + " " + user.LastName + "\n";
                message += "\n";
                message += "TwojeDane\n";
                message += "Login - " + user.Login;
                message += "Haslo - " + user.Password;
                message += "\n";
                message += "Z Powazaniem \nVet Clinic";
                EMaill eMail = new EMaill(user.Email, "Vet Clinic rejestracja",message);
                eMail.send();
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