using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Intranet.Controllers
{
    public class UserTypeController : Controller
    {
        private readonly VetClinicContext _context;

        public UserTypeController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: UserTypes
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.UserTypes.Include(u => u.UserTypeAddedUser).Include(u => u.UserTypeUpdatedUser);
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: UserTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userType = await _context.UserTypes
                .Include(u => u.UserTypeAddedUser)
                .Include(u => u.UserTypeUpdatedUser)
                .FirstOrDefaultAsync(m => m.UserTypeID == id);
            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        // GET: UserTypes/Create
        public IActionResult Create()
        {
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            return View();
        }

        // POST: UserTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserTypeID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] UserType userType)
        {
            if (ModelState.IsValid)
            {
                userType.AddedDate = DateTime.Now;
                _context.Add(userType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", userType.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", userType.UpdatedUserID);
            return View(userType);
        }

        // GET: UserTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userType = await _context.UserTypes.FindAsync(id);
            if (userType == null)
            {
                return NotFound();
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", userType.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", userType.UpdatedUserID);
            return View(userType);
        }

        // POST: UserTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserTypeID,Name,Description,IsActive,AddedDate,AddedUserID,UpdatedUserID")] UserType userType)
        {
            if (id != userType.UserTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userType.UpdatedDate = DateTime.Now;
                    _context.Update(userType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTypeExists(userType.UserTypeID))
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
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", userType.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", userType.UpdatedUserID);
            return View(userType);
        }

        // GET: UserTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userType = await _context.UserTypes
                .Include(u => u.UserTypeAddedUser)
                .Include(u => u.UserTypeUpdatedUser)
                .FirstOrDefaultAsync(m => m.UserTypeID == id);
            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        // POST: UserTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userType = await _context.UserTypes.FindAsync(id);
            //_context.UserTypes.Remove(userType);
            userType.IsActive = false;
            userType.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTypeExists(int id)
        {
            return _context.UserTypes.Any(e => e.UserTypeID == id);
        }
    }
}
