﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Helpers;

namespace VetClinic.Intranet.Controllers
{
    public class CustomersController : Controller
    {
        private readonly VetClinicContext _context;
        private readonly string CustomerUserName = "Klient";
        private readonly int CustomerUserId = 4;
        SmtpConfiguration SmtpConf = new SmtpConfiguration(); // konfuguracja smtp do wysyłki maila
        public CustomersController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var vetClinicContext = _context.Users.Include(u => u.UserType).Where(u => u.UserType.Name == CustomerUserName);
            if (!String.IsNullOrEmpty(searchString))
            {
                vetClinicContext = (from user in vetClinicContext
                                    where user.Login.Contains(searchString)
                                    || user.LastName.Contains(searchString)
                                    || user.Email.Contains(searchString)
                                    || user.City.Contains(searchString)
                                    select user)
                                            .Include(m => m.UserType);


            }
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.UserType)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name");
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,UserTypeID,FirstName,LastName,HouseNumber,ApartmentNumber,Street,City,PostalCode,Email,Login,Password,Phone,Photo,CardNumber,IsActive,Description")] User user, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                user.AddedDate = DateTime.Now;
                user.IsActive = true;
                user.UserTypeID = CustomerUserId;
                var passwordUser = user.Password;
                user.Password = HashPassword.GetMd5Hash(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                UploadPhoto(file, user.UserID);
                SmtpConf.MessageTo = user.Email;
                SmtpConf.MessageText = user.FirstName + " Dane logowania: " + "<br>" + "Login: " + user.Login + "<br>" + "Hasło: " + passwordUser;
                SmtpConf.MessageSubject = "Potwierdzenie dokonanej rejestracji";
                SmtpConf.send();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserTypeID,FirstName,LastName,HouseNumber,ApartmentNumber,Street,City,PostalCode,Email,Login,Password,Phone,Photo,CardNumber,IsActive,Description")] User user, IFormFile file)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.UpdatedDate = DateTime.Now;
                    user.UserTypeID = CustomerUserId;
                    user.Password = HashPassword.GetMd5Hash(user.Password);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    UploadPhoto(file, user.UserID);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.UserType)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            user.IsActive = false;
            user.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
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
    }
}
