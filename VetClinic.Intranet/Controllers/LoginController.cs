using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Helpers;

namespace VetClinic.Intranet.Controllers
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

            User account = _context.Users.FirstOrDefault(u => u.Login == user.Login);
            if (account != null)
            {
                if (account.IsActive == false)
                {
                    ModelState.AddModelError("", "Twoje konto jest zablokowane.");
                    return View();
                }
                else
                if (HashPassword.VerifyMd5Hash(user.Password, account.Password))
                {
                    HttpContext.Session.SetString("UserID", account.UserID.ToString());
                    HttpContext.Session.SetString("Login", account.Login.ToString());
                    account.LoginAttempt = 0;
                    _context.SaveChanges();

                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("","Błędny login lub hasło");

                    account.LoginAttempt++;
                    _context.SaveChanges();
                    if (account.LoginAttempt > 4)
                    {
                        account.IsActive = false;
                        _context.SaveChanges();
                        ModelState.AddModelError(" ","Twoje konto jest zablokowane");
                    }
                }
                return View();
            }
            if (account == null)
            {
                ModelState.AddModelError("","Błędny login lub hasło");
                return View();
            }
            else
                return View();

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}