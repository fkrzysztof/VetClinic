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
using User = VetClinic.Data.Data.Clinic.User;

namespace VetClinic.Intranet.Controllers
{
    public class UsersController : Controller
    {
        private readonly VetClinicContext _context;

        public UsersController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.Users.Include(u => u.UserType);
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: Users/Details/5
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

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name");
            return View();
        }

        // POST: Users/Create
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
                _context.Add(user);
                await _context.SaveChangesAsync();
                UploadPhoto(file, user.UserID);
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", user.UserTypeID);
            return View(user);
        }

        // GET: Users/Edit/5
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

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,AddedDate,UserTypeID,FirstName,LastName,HouseNumber,ApartmentNumber,Street,City,PostalCode,Email,Login,Password,Phone,Photo,CardNumber,IsActive,LoginAttempt,Description")] User user, IFormFile file)
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

        // GET: Users/Delete/5
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

        // POST: Users/Delete/5
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
