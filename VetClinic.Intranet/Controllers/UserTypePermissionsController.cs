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
    public class UserTypePermissionsController : Controller
    {
        private readonly VetClinicContext _context;

        public UserTypePermissionsController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: UserTypePermissions
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.UserTypePermissions.Include(u => u.Permission).Include(u => u.UserType).Include(u => u.UserTypePermissionAddedUser).Include(u => u.UserTypePermissionUpdatedUser);
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: UserTypePermissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTypePermission = await _context.UserTypePermissions
                .Include(u => u.Permission)
                .Include(u => u.UserType)
                .Include(u => u.UserTypePermissionAddedUser)
                .Include(u => u.UserTypePermissionUpdatedUser)
                .FirstOrDefaultAsync(m => m.UserPermissionID == id);
            if (userTypePermission == null)
            {
                return NotFound();
            }

            return View(userTypePermission);
        }

        // GET: UserTypePermissions/Create
        public IActionResult Create()
        {
            ViewData["PermissionID"] = new SelectList(_context.Set<Permission>(), "PermissionID", "Name");
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name");
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            return View();
        }

        // POST: UserTypePermissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserPermissionID,UserTypeID,PermissionID,Access,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] UserTypePermission userTypePermission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userTypePermission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PermissionID"] = new SelectList(_context.Set<Permission>(), "PermissionID", "Name", userTypePermission.PermissionID);
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", userTypePermission.UserTypeID);
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", userTypePermission.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", userTypePermission.UpdatedUserID);
            return View(userTypePermission);
        }

        // GET: UserTypePermissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTypePermission = await _context.UserTypePermissions.FindAsync(id);
            if (userTypePermission == null)
            {
                return NotFound();
            }
            ViewData["PermissionID"] = new SelectList(_context.Set<Permission>(), "PermissionID", "Name", userTypePermission.PermissionID);
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", userTypePermission.UserTypeID);
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", userTypePermission.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", userTypePermission.UpdatedUserID);
            return View(userTypePermission);
        }

        // POST: UserTypePermissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserPermissionID,UserTypeID,PermissionID,Access,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] UserTypePermission userTypePermission)
        {
            if (id != userTypePermission.UserPermissionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTypePermission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTypePermissionExists(userTypePermission.UserPermissionID))
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
            ViewData["PermissionID"] = new SelectList(_context.Set<Permission>(), "PermissionID", "Name", userTypePermission.PermissionID);
            ViewData["UserTypeID"] = new SelectList(_context.UserTypes, "UserTypeID", "Name", userTypePermission.UserTypeID);
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", userTypePermission.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", userTypePermission.UpdatedUserID);
            return View(userTypePermission);
        }

        // GET: UserTypePermissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTypePermission = await _context.UserTypePermissions
                .Include(u => u.Permission)
                .Include(u => u.UserType)
                .Include(u => u.UserTypePermissionAddedUser)
                .Include(u => u.UserTypePermissionUpdatedUser)
                .FirstOrDefaultAsync(m => m.UserPermissionID == id);
            if (userTypePermission == null)
            {
                return NotFound();
            }

            return View(userTypePermission);
        }

        // POST: UserTypePermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userTypePermission = await _context.UserTypePermissions.FindAsync(id);
            _context.UserTypePermissions.Remove(userTypePermission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTypePermissionExists(int id)
        {
            return _context.UserTypePermissions.Any(e => e.UserPermissionID == id);
        }
    }
}
