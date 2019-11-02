using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.Intranet.Controllers
{
    public class UsersRegistrationController : Controller
    {
        private readonly VetClinicContext _context;

        public UsersRegistrationController(VetClinicContext context)
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
                return View("End");
            }
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
        }
    }
}
