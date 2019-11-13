using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VetClinic.Data;
using VetClinic.Data.Data.VetClinic;
using VetClinic.Intranet.Services;

namespace VetClinic.Intranet.Controllers
{
    public class EmployeesRegistrationController : Controller
    {
        private readonly VetClinicContext _context;
        private readonly string EmployeeUserName = "Pracownik";
        private readonly int EmployeeUserId = 3;
        SmtpConfiguration SmtpConf = new SmtpConfiguration(); // konfuguracja smtp do wysyłki maila

        public EmployeesRegistrationController(VetClinicContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View("/Views/Home/Index.cshtml");
        }
        public IActionResult Create()
        {
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name");
            return View("/Views/Employees/Create.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,UserTypeID,FirstName,LastName,HouseNumber,ApartmentNumber,Street,City,PostalCode,Email,Login,Password,Phone,Photo,CardNumber,IsActive,Description")] User user, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                user.AddedDate = DateTime.Now;
                user.IsActive = true;
                user.UserTypeID = EmployeeUserId;
                var hasloDlaUrzytkownika = user.Password;
                var salt = Salt.Create();
                var hash = Hash.Create(user.Password, salt);
                user.Password = hash;
                _context.Add(user);
                await _context.SaveChangesAsync();
                UploadPhoto(file, user.UserID);
                SmtpConf.MessageTo = user.Email;
                SmtpConf.MessageText = user.FirstName + " witamy w zespole :)" + "<br>" + "Login: " + user.Login + "<br>" + "Hasło: " + hasloDlaUrzytkownika;
                SmtpConf.MessageSubject = "Potwierdzenie dokonanej rejestracji";
                SmtpConf.send();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
        }
        //Upload photo
        public void UploadPhoto(IFormFile file, int id)
        {
            if (file != null)
            {
                var fileName = file.FileName;

                var path = Path.Combine(Directory.GetCurrentDirectory(), "../VetClinic.Intranet/wwwroot/uploads", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                var user =
                    (from item in _context.Users
                     where item.UserID == id
                     select item
                    ).FirstOrDefault();

                user.Photo = fileName;
                _context.Update(user);
                _context.SaveChanges();
            }
        }
        // Haszowanie hasła
        public class Hash
        {
            public static string Create(string value, string salt)
            {
                var valueBytes = KeyDerivation.Pbkdf2(
                                    password: value,
                                    salt: Encoding.UTF8.GetBytes(salt),
                                    prf: KeyDerivationPrf.HMACSHA512,
                                    iterationCount: 10000,
                                    numBytesRequested: 256 / 8);

                return Convert.ToBase64String(valueBytes);
            }
            public static bool Validate(string value, string salt, string hash)
                    => Create(value, salt) == hash;
        }
        public class Salt
        {

            public static string Create()
            {
                byte[] randomBytes = new byte[128 / 8];
                using (var generator = RandomNumberGenerator.Create())
                {
                    generator.GetBytes(randomBytes);
                    return Convert.ToBase64String(randomBytes);
                }
            }

        }
        
    }
}