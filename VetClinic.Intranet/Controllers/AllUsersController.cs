using System;
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
using VetClinic.Intranet.Controllers.Abstract;

namespace VetClinic.Intranet.Controllers
{
    public class AllUsersController : AbstractPolicyController
    {
        protected int _userTypeId;
        SmtpConfiguration SmtpConf = new SmtpConfiguration();
        public AllUsersController(VetClinicContext context) : base(context) { }

        // GET: Admin
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var vetClinicContext = _context.Users.Include(u => u.UserType);
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
            return View(await vetClinicContext.OrderByDescending(u => u.IsActive).ThenByDescending(u => u.UpdatedDate).ToListAsync());
        }


        // GET: Admin/Edit/5
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
            ViewBag.IdUser = user.UserID;
            ViewBag.Type = user.UserTypeID;
            ViewBag.MedicalSpecializations = _context.MedicalSpecializations.Include(m => m.MedicalSpecializationUser).Include(m => m.Specialization).Where(w => w.UserID == id);
            return View(user);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserTypeID,FirstName,LastName,HouseNumber,ApartmentNumber,Street,City,PostalCode,Email,Login,Password,Phone,Photo,Image,CardNumber,IsActive,Description")] User user, List<Microsoft.AspNetCore.Http.IFormFile> Image)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in Image)
                    {
                        using (var stream = new MemoryStream())
                        {
                            if (item != null)
                            {
                                await item.CopyToAsync(stream);
                                user.Image = stream.ToArray();
                            }
                        }
                    }

                    user.UpdatedDate = DateTime.Now;
                    user.IsActive = true;
                    user.AuthorizationEmail = true;
                    //.UserTypeID = _userTypeId;
                    if (user.Password.Length == 8)
                    {
                        var passwordUser = user.Password;
                        user.Password = HashPassword.GetMd5Hash(user.Password);
                        SmtpConf.MessageTo = user.Email;
                        SmtpConf.MessageText = user.FirstName + " Twoje nowe hasło: " + passwordUser;
                        SmtpConf.MessageSubject = "Potwierdzenie zmiany hasła";
                        SmtpConf.send();
                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();
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

            return View(user);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            user.IsActive = false;
            user.LoginAttempt = 5;
            user.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            user.IsActive = true;
            user.LoginAttempt = 0;
            user.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }

        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(string[] ids)
        {
            int[] getid = null;
            if (ids != null)
            {
                getid = new int[ids.Length];
                int j = 0;
                foreach (string i in ids)
                {
                    int.TryParse(i, out getid[j++]);
                }
            }

            List<User> getusrids = new List<User>();
            getusrids = _context.Users.Where(x => getid.Contains(x.UserID)).ToList();
            foreach (var s in getusrids)
            {
                _context.Users.Update(s);
                s.IsActive = false;
                s.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult lockUnlock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.LoginAttempt > 4)
            {
                user.IsActive = true;
                user.LoginAttempt = 0;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            user.IsActive = false;
            user.LoginAttempt = 5;
            user.UpdatedDate = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
