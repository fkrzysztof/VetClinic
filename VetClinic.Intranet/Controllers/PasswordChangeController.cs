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
    public class PasswordChangeController : Controller
    {
        private readonly VetClinicContext _context;
        SmtpConfiguration SmtpConf = new SmtpConfiguration();

        public PasswordChangeController(VetClinicContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string Password, string NewPassword, string NewPasswordConfirm)
        {
            var test = ModelState.IsValid;
            var UserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
            var newPassword = NewPassword.Equals(NewPasswordConfirm);
            var crrentPassword = HashPassword.VerifyMd5Hash(Password, _context.Users.Where(u=>u.UserID==UserID).Select(p=>p.Password).First());

            var length = NewPassword.Length;
            if (length < 7)
            {
                ModelState.AddModelError("","Haslo musi mieć conajmniej 8 znaków");
                return View();
            }
            var passCharArray = NewPassword.ToCharArray();
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
                        special = true;break;
                }
            }
            if (!special)
            {
                ModelState.AddModelError("", "Haslo musi mieć conajmniej jeden znak specjalny");
                return View();
            }

            if (ModelState.IsValid&&newPassword&&crrentPassword)
            {
                _context.Users.Find(UserID).Password = HashPassword.GetMd5Hash(NewPassword);

                var usersEmail =
                        (from users in _context.Users
                         where users.UserID == UserID
                         select users.Email
                         ).FirstOrDefault();

                //var usersToken =
                //        (from users in _context.Users
                //         where users.UserID == UserID
                //         select users.ActivationToken
                //         ).FirstOrDefault();

                SmtpConf.MessageTo = usersEmail;
                SmtpConf.MessageText = "Witaj <br>"
                                        + "Twoje hasło zostało właśnie zmienione. <br>"
                                        + "<br>"

                                        + "Pozdrawiamy <br>"
                                        + "Zespół VetClinic <br>";

                //+ "Jeżeli to Ty dokonałeś zmiany hasła możesz zignorować tą wiadomość: <br>"
                //+ "Jeśli uważasz, że ktoś mógł włamać się na twoje konto i zmienić twoje hasło, możesz je zresetować klikając w poniższy link: <br>"
                //+ "Link do zmiany hasła: https://vetclinic-portalwww.azurewebsites.net/Login/ResetPassword?email=" + usersEmail + "&token=" + usersToken + "";

                SmtpConf.MessageSubject = "Potwierdzenie zmiany hasła";

                _context.Users.Find(UserID).UpdatedDate = DateTime.Now;
                _context.Users.Find(UserID).IsActive = true;
                _context.Users.Find(UserID).AuthorizationEmail = true;

                _context.SaveChanges();

                SmtpConf.send();
                TempData["msg"] = "<script>Swal.fire({icon: 'success', title:'Hasło zostało zmienione.'});</script>";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        }
}