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
            User account = _context.Users.FirstOrDefault(u => u.Login.ToLower() == user.Login.ToLower());

            if (account == null)
            {
                ModelState.AddModelError("", "Niepoprawny login!");
                return View();
            }

            var veryfyHashPassword = HashPassword.VerifyMd5Hash(user.Password, account.Password);

            if (account.AuthorizationEmail == false)
            {
                ModelState.AddModelError("", "Twoje konto nie zostało jeszcze aktywowane. Sprawdź swoją skrzynkę pocztową!");
                return View();
            }

            if (account.IsActive == false)
            {
                ModelState.AddModelError("", "Twoje konto jest zablokowane.");
                return View();
            }
            else if (account != null && (account.Password == user.Password || veryfyHashPassword) && account.Login.ToLower() == user.Login.ToLower())
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
            var rnd = new Random();
            string login;
            string email;

            login = user.Login;
            email = user.Email;

            var usersLogin =
                (from users in _context.Users
                 select users.Login
                 ).ToList().ConvertAll(d => d.ToLower());

            var usersEmail =
                (from users in _context.Users
                 select users.Email
                 ).ToList().ConvertAll(d => d.ToLower());

            // check email
            if (usersEmail.Contains(email.ToLower()))
            {
                ModelState.AddModelError("", "Konto z takim adresem email już istnieje!");
                return View();
            }

            //check login
            for (int i = 0; i < usersLogin.Count(); i++)
            {
                if (usersLogin.Contains(login.ToLower()))
                {
                    login += rnd.Next(0, 9);
                    ModelState.AddModelError("", "Taki login już istnieje! Wybierz inny: " + login.ToLower());
                    return View();
                }
                else
                {
                    break;
                }
            }

            // validation password
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

            if(Password != ConfirmPassword)
            {
                ModelState.AddModelError("", "Hasła nie są takie same");
                return View();
            }

            var typeid =
                    (from item in _context.UserTypes
                     where item.Name == "Klient" || item.Name == "Klienci"
                     select item.UserTypeID
                     ).FirstOrDefault();

            var token = Guid.NewGuid().ToString();

            if (ModelState.IsValid)
            {
                user.Login = login.ToLower();
                user.Email = email.ToLower();

                SmtpConf.MessageTo = user.Email;
                SmtpConf.MessageText = user.FirstName + " witamy w zespole " + "<br>" + "<br>" + " Twój login: " + user.Login + "<br>"
                                        + "<br>"
                                        + "Link aktywacyjny: https://vetclinic-portalwww.azurewebsites.net/Login/AccountConfirm?email=" + user.Email + "&token=" + token + "<br>"
                                        + "<br>"
                                        + "Pozdrawiamy <br>"
                                        + "Zespół VetClinic";
                // vetclinic-portalwww.azurewebsites.net
                // localhost:44343
                SmtpConf.MessageSubject = "Potwierdzenie dokonanej rejestracji";

                user.Password = HashPassword.GetMd5Hash(user.Password);

                user.UserTypeID = typeid;
                user.AddedDate = DateTime.Now;
                user.ActivationToken = token;
                user.IsActive = false;
                user.AuthorizationEmail = false;
                user.LoginAttempt = 0;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                ModelState.Clear();

                SmtpConf.send();

                return RedirectToAction("RegisterSucces", "Login");
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
        public IActionResult RegisterSucces()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterSucces(User user)
        {
            string email;
            email = user.Email;

            var usersEmail =
                (from users in _context.Users
                 select users.Email
                 ).ToList();

            var usersToken =
                (from users in _context.Users
                 where users.Email == email
                 select users.ActivationToken
                 ).FirstOrDefault();

            var usersName =
                (from users in _context.Users
                 where users.Email == email
                 select users.FirstName
                 ).FirstOrDefault();

            if (usersEmail.Contains(email))
            {

                SmtpConf.MessageTo = user.Email;
                SmtpConf.MessageText = "Witaj " + usersName + " to twój link aktywacyjny. Tym razem go nie zgub :P" + "<br>"
                                            + "Link aktywacyjny: https://vetclinic-portalwww.azurewebsites.net/Login/AccountConfirm?email=" + user.Email + "&token=" + usersToken + "<br>"
                                            + "<br>"
                                            + "Pozdrawiamy <br>"
                                            + "Zespół VetClinic";

                // vetclinic-portalwww.azurewebsites.net
                // localhost:44343

                SmtpConf.MessageSubject = "Vet Clinic - Link aktywacyjny";

                SmtpConf.send();

                ModelState.AddModelError("", "Wiadomość została wysłana");

                return View();
            }
            else
            {
                ModelState.AddModelError("", "Niepoprawny adres email");
                return View();
            }
            
        }


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
                (from users in _context.Users
                 where users.Email == email
                 select users.Email
                 ).FirstOrDefault();

            var usersToken =
                (from users in _context.Users
                 where users.ActivationToken == token
                 select users.ActivationToken
                 ).FirstOrDefault();

            var usersEmailStatus =
                (from users in _context.Users
                 where users.ActivationToken == token && users.Email == email
                 select users.AuthorizationEmail
                 ).FirstOrDefault();


            if (usersEmail == email && usersToken == token && usersEmailStatus == true)
            {
                ViewData["Result"] = "Twoje konto jest już aktywne!";
                ViewData["Status"] = "true";
                return View();
            }
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
                    s.AuthorizationEmail = true;
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

        // send email to restet password
        public IActionResult RemindPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemindPassword(User user)
        {
            string email;
            email = user.Email;

            var usersEmail =
                (from users in _context.Users
                 where users.Email == email
                 select users.Email
                 ).FirstOrDefault();

            var usersToken =
                (from users in _context.Users
                 where users.Email == email
                 select users.ActivationToken
                 ).FirstOrDefault();

            if (usersEmail == email && email != null)
            {
                SmtpConf.MessageTo = usersEmail;
                SmtpConf.MessageText = "Witaj <br>"
                                       + "<br>"
                                       + "Wysłano prośbę o zmianę hasła. Jeżeli to Ty klikni w poniższy link <br>"
                                       + "Jeśli uważasz, że to pomyłka możesz zignorować tą wiadomość <br>"
                                       + "<br>"
                                       + "Zresetuj hasło -> https://vetclinic-portalwww.azurewebsites.net/Login/ResetPassword?email=" + usersEmail + "&token=" + usersToken + "<br>"
                                       + "<br>"
                                       + "Pozdrawiamy <br>"
                                       + "Zespół VetClinic";

                // vetclinic-portalwww.azurewebsites.net
                // localhost:44343

                SmtpConf.MessageSubject = "Resetowanie hasła";
                SmtpConf.send();

                ModelState.AddModelError("", "Wiadomość została wysłana. Sprawdź swoją skrzynkę pocztową.");

                return View();
            }
            else
            {
                ModelState.AddModelError("", "Niepoprawny adres email");
                return View();
            }
        }

        // reset password
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if(token == null || email == null)
            {
                ModelState.AddModelError("", "Błędny token resetujący");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(User user, string Password, string ConfirmPassword)
        {
            string email;
            email = user.Email;

            string token;
            token = user.ActivationToken;

            Uri myUri = new Uri("https://vetclinic-portalwww.azurewebsites.net/Login/ResetPassword?email=" + email + "&token=" + token);

            string userEmail = HttpUtility.ParseQueryString(myUri.Query).Get("email");
            //string userToken = HttpUtility.ParseQueryString(myUri.Query).Get("token");

            //var crrentEmail = _context.Users.Where(u => u.Email == email).Select(p => p.UserID).FirstOrDefault();

            var usersEmail =
                (from users in _context.Users
                 where users.Email == email
                 select users.Email
                 ).FirstOrDefault();

            //var usersToken =
            //    (from users in _context.Users
            //     where users.ActivationToken == token
            //     select users.ActivationToken
            //     ).FirstOrDefault();

            // validation password
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

            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError("", "Hasła nie są takie same");
                return View();
            }

            if (user.Email == userEmail  && user.Password != null)
            {
                List<User> getusrids = new List<User>();
                getusrids = _context.Users.Where(x => usersEmail.Contains(x.Email)).ToList();
                foreach (var s in getusrids)
                {
                    _context.Users.Update(s);
                    s.Password = HashPassword.GetMd5Hash(Password);
                    s.UpdatedDate = DateTime.Now;
                    
                    SmtpConf.MessageTo = usersEmail;
                    SmtpConf.MessageText = "Witaj <br>"
                                            + "<br>"
                                            + "Twoje hasło zostało właśnie zmienione. <br>"
                                            + "<br>"
                                            + "Pozdrawiamy <br>"
                                            + "Zespół VetClinic";

                    SmtpConf.MessageSubject = "Potwierdzenie zmiany hasła";

                    await _context.SaveChangesAsync();
                    SmtpConf.send();
                }

                return View("ResetPasswordConfirm", "Login");
            }

            ModelState.AddModelError("", "Nie zmieniono hasła");
            return View();
        }
    }
}
