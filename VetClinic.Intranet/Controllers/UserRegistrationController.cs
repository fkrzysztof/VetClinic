using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.VetClinic;
using VetClinic.Intranet.Services;

namespace VetClinic.Intranet.Controllers
{
    public class UserRegistrationController : Controller
    {
        private readonly VetClinicContext _context;
        SmtpConfiguration SmtpConf = new SmtpConfiguration(); // konfuguracja smtp do wysyłki maila

        public UserRegistrationController(VetClinicContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name");
            return View();
        }

        // POST: UsersRegistration/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,UserTypeID,FirstName,LastName,Address,City,PostalCode,Email,Login,Password,Phone,Photo,CardNumber,IsActive,LoginAttempt,Description,AddedDate,UpdatedDate")] User user)
        {
            if (ModelState.IsValid)
            {
                user.AddedDate = DateTime.Now;
                ViewBag.Nazwisko = user.FirstName;
                _context.Add(user);
                await _context.SaveChangesAsync();
                // wysyłamy maila do uzytkownika z potwierdzeniem rejestarcji
                SmtpConf.MessageTo = user.Email;
                SmtpConf.MessageText = user.FirstName+" witamy w zespole :)"+"<br>"+"Login: "+user.Login+"<br>"+"Hasło: "+user.Password;
                SmtpConf.MessageSubject = "Potwierdzenie dokonanej rejestracji";
                SmtpConf.send();
                return View("End");
            }
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
        }
        //Generowanie hasła użytkownikowi
        public static class Password
        {
            private static readonly char[] Punctuations =
            "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();

            public static string Generate(int length, int numberOfNonAlphanumericCharacters)
            {
                if (length < 1 || length > 128)
                {
                    throw new ArgumentException(nameof(length));
                }

                if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
                {
                    throw new ArgumentException(nameof(numberOfNonAlphanumericCharacters));
                }

                using (var rng = RandomNumberGenerator.Create())
                {
                    var byteBuffer = new byte[length];

                    rng.GetBytes(byteBuffer);

                    var count = 0;
                    var characterBuffer = new char[length];

                    for (var iter = 0; iter < length; iter++)
                    {
                        var i = byteBuffer[iter] % 87;

                        if (i < 10)
                        {
                            characterBuffer[iter] = (char)('0' + i);
                        }
                        else if (i < 36)
                        {
                            characterBuffer[iter] = (char)('A' + i - 10);
                        }
                        else if (i < 62)
                        {
                            characterBuffer[iter] = (char)('a' + i - 36);
                        }
                        else
                        {
                            characterBuffer[iter] = Punctuations[i - 62];
                            count++;
                        }
                    }

                    if (count >= numberOfNonAlphanumericCharacters)
                    {
                        return new string(characterBuffer);
                    }

                    int j;
                    var rand = new Random();

                    for (j = 0; j < numberOfNonAlphanumericCharacters - count; j++)
                    {
                        int k;
                        do
                        {
                            k = rand.Next(0, length);
                        }
                        while (!char.IsLetterOrDigit(characterBuffer[k]));

                        characterBuffer[k] = Punctuations[rand.Next(0, Punctuations.Length)];
                    }

                    return new string(characterBuffer);
                }
            }
        }

        public string Haslo()
        {
            return Password.Generate(32, 12);
        }
    }
}
