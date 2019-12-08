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

namespace VetClinic.Intranet.Controllers
{
    public class AdminsController : Controller
    {
        private readonly VetClinicContext _context;

        private readonly string AdminUserName = "admin";
        private readonly int AdminUserId = 1;
        public AdminsController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var vetClinicContext = _context.Users.Include(u => u.UserType).Where(u => u.UserType.Name == AdminUserName && u.IsActive == true);
            if (!String.IsNullOrEmpty(searchString))
            {
               vetClinicContext =  (from user in vetClinicContext
                 where user.Login.Contains(searchString)
                 || user.LastName.Contains(searchString)
                 || user.Email.Contains(searchString)
                 || user.City.Contains(searchString)
                                    select user)
                                           .Include(m => m.UserType);


            }
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: Admin/Details/5
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

        // GET: Admin/Create
        public IActionResult Create()
        {
            
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user, List<Microsoft.AspNetCore.Http.IFormFile> Image)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in Image)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        user.Image = stream.ToArray();
                    }
                }
                user.AddedDate = DateTime.Now;
                user.IsActive = true;
                user.UserTypeID = AdminUserId;             
                user.Password = HashPassword.GetMd5Hash(user.Password);                       
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
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
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", user.UserTypeID);
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
                    user.UserTypeID = AdminUserId;
                    if (user.Password.Length == 8)
                    {
                        user.Password = HashPassword.GetMd5Hash(user.Password);
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
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
        }

        // GET: Admin/Delete/5
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

        // POST: Admin/Delete/5
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
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
