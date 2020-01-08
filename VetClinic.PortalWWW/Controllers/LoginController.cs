using System;

using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace VetClinic.PortalWWW.Controllers
{
    public class LoginController : Controller
    {
        private readonly VetClinicContext _context;

        SmtpConfiguration SmtpConf = new SmtpConfiguration();

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

            if (account == null)
            {
                ModelState.AddModelError("", "Niepoprawny login!");
                return View();
            }

            var veryfyHashPassword = HashPassword.VerifyMd5Hash(user.Password, account.Password);

            if (account.ActivationToken != "activate")
            {
                ModelState.AddModelError("", "Twoje konto nie zostało jeszcze aktywowane. Sprawdź swoją skrzynkę pocztową!");
                return View();
            }

            if (account.IsActive == false)
            {
                ModelState.AddModelError("", "Twoje konto jest zablokowane.");
                return View();
            }
            else if (account != null && (account.Password == user.Password || veryfyHashPassword) && account.Login == user.Login)
            {
                HttpContext.Session.SetString("UserID", account.UserID.ToString());
                HttpContext.Session.SetString("Login", account.Login.ToString());
                account.LoginAttempt = 0;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "ClientPanel");
            }
            else
            {
                ModelState.AddModelError("", "Hasło jest niepoprawne.");
                account.LoginAttempt++;
                _context.SaveChanges();
                if (account.LoginAttempt >= 5)
                {
                    account.IsActive = false;
                    _context.SaveChanges();
                    ModelState.AddModelError("", "Twoje konto zostalo zablokowane.");
                }
            }

            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(User user, string Password, string ConfirmPassword)
        {
            // generowanie loginu
            var rnd = new Random();
            string login;

            login = user.Login;

            var users =
                (from uzytkownicy in _context.Users
                 select uzytkownicy.Login
                 ).ToList();

            for (int i = 0; i < users.Count(); i++)
            {
                if (users.Contains(login))
                {
                    login += rnd.Next(0, 9);
                    ModelState.AddModelError("", "Taki login już istnieje! Wybierz inny: " + login);
                    return View();
                }
                else
                {
                    break;
                }
            }
            // ====================

            // sprawdzanie hasła
            var length = user.Password.Length;
            if (length < 7)
            {
                ModelState.AddModelError("", "Haslo musi mieć conajmniej 8 znaków");
                return View();
            }
            var passCharArray = user.Password.ToCharArray();
            var capitalLeter = false;
            foreach (Char c in passCharArray)
            {
                if (Char.IsUpper(c))
                {
                    capitalLeter = true;
                }
            }
            if (!capitalLeter)
            {
                ModelState.AddModelError("", "Haslo musi mieć conajmniej jedną dużą Literę");
                return View();
            }
            var lowerCase = false;
            foreach (Char c in passCharArray)
            {
                if (Char.IsLower(c))
                {
                    lowerCase = true;
                }
            }
            if (!lowerCase)
            {
                ModelState.AddModelError("", "Haslo musi mieć conajmniej jedną małą Literę");
                return View();
            }
            var digit = false;
            foreach (Char c in passCharArray)
            {
                if (Char.IsDigit(c))
                {
                    digit = true;
                }
            }
            if (!digit)
            {
                ModelState.AddModelError("", "Haslo musi mieć conajmniej jedną cyfrę");
                return View();
            }
            var special = false;
            foreach (Char c in passCharArray)
            {
                switch (c)
                {
                    case '!':
                    case '@':
                    case '#':
                    case '$':
                    case '%':
                    case '^':
                    case '&':
                    case '*':
                        special = true; break;
                }
            }
            if (!special)
            {
                ModelState.AddModelError("", "Haslo musi mieć conajmniej jeden znak specjalny");
                return View();
            }
            //if (Password != ConfirmPassword)
            //{
            //    ModelState.AddModelError("", "Hasła nie są taki same");
            //    return View();
            //}

            var typeid =
                    (from item in _context.UserTypes
                     where item.Name == "Klient" || item.Name == "Klienci"
                     select item.UserTypeID
                     ).FirstOrDefault();

            var token = Guid.NewGuid().ToString();

            if (ModelState.IsValid)
            {
                user.Login = login;

                SmtpConf.MessageTo = user.Email;
                SmtpConf.MessageText = user.FirstName + " witamy w zespole :)" + "<br>" + "Login: " + user.Login + "<br>" + "Hasło: " + user.Password + "<br>"
                                        + "Link aktywacyjny: https://localhost:44343/Login/AccountConfirm?email=" + user.Email + "&token=" + token + "";
                SmtpConf.MessageSubject = "Potwierdzenie dokonanej rejestracji";

                user.Password = HashPassword.GetMd5Hash(user.Password);

                user.UserTypeID = typeid;
                user.AddedDate = DateTime.Now;
                user.ActivationToken = token;
                user.IsActive = false;
                user.LoginAttempt = 0;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                ModelState.Clear();

                SmtpConf.send();

                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        //public IActionResult AccountConfirm()
        //{
        //    return View();
        //}

        
        public async Task<IActionResult> AccountConfirm(string email, string token)
        {
            ViewData["Email"] = email;
            ViewData["Token"] = token;

            var vetClinicContext = _context.Users.Include(u => u.UserType);
            if (!String.IsNullOrEmpty(email) || !String.IsNullOrEmpty(token))
            {
                vetClinicContext = (from user in vetClinicContext
                                    where user.Email.Contains(email)
                                    && user.ActivationToken.Contains(token)
                                    select user)
                                            .Include(m => m.UserType);
            }

            var usersEmail =
                (from uzytkownicy in _context.Users
                 where uzytkownicy.Email == email
                 select uzytkownicy.Email
                 ).FirstOrDefault();

            var usersToken =
                (from uzytkownicy in _context.Users
                 where uzytkownicy.ActivationToken == token
                 select uzytkownicy.ActivationToken
                 ).FirstOrDefault();

            //if (usersEmail == email && usersToken == null)
            //{
            //    ViewData["Result"] = "Twoje konto jest już aktywne!";
            //    return View();
            //}
            if (usersEmail == email && usersToken == token)
            {
                ViewData["Result"] = "Twój adres email " + email  + " został poprawnie zweryfikowany!";
                ViewData["Status"] = "true";

                List<User> getusrids = new List<User>();
                getusrids = _context.Users.Where(x => email.Contains(x.Email)).Where(x => token.Contains(x.ActivationToken)).ToList();
                foreach (var s in getusrids)
                {
                    _context.Users.Update(s);
                    s.IsActive = true;
                    s.ActivationToken = "activate";
                    s.UpdatedDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                }

                return View(await vetClinicContext.ToListAsync());
            }
            else
            {
                ViewData["Result"] = "Twój adres email " + email + " nie został poprawnie zweryfikowany!";

                return View();
            }
        } 
    }
}
