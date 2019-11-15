﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Intranet.Controllers
{
    public class PermissionsController : Controller
    {
        private readonly VetClinicContext _context;

        public PermissionsController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: Permissions
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.Permissions.Include(p => p.PermissionAddedUser).Include(p => p.PermissionUpdatedUser).Where(w => w.IsActive == true);
            return View(await vetClinicContext.ToListAsync());
        }


        // GET: Permissions/Create
        public IActionResult Create()
        {
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            return View();
        }

        // POST: Permissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PermissionID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Permission permission)
        {
            permission.IsActive = true;
            permission.AddedDate = DateTime.Now;
            
            if (ModelState.IsValid)
            {
                _context.Add(permission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", permission.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", permission.UpdatedUserID);
            return View(permission);
        }

        // GET: Permissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null)
            {
                return NotFound();
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", permission.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", permission.UpdatedUserID);
            return View(permission);
        }

        // POST: Permissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PermissionID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Permission permission)
        {

            if (id != permission.PermissionID)
            {
                return NotFound();
            }

            permission.IsActive = true;
            permission.UpdatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermissionExists(permission.PermissionID))
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
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", permission.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", permission.UpdatedUserID);
            return View(permission);
        }


        // POST: Permissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permission = await _context.Permissions.FindAsync(id);

            permission.IsActive = false;
            _context.SaveChanges();

            var permissionList = _context.UserTypePermissions.Where(w => w.PermissionID == id).ToList();
            foreach (var itemPermissionType in permissionList)
            {
                itemPermissionType.IsActive = false;
                _context.SaveChanges();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool PermissionExists(int id)
        {
            return _context.Permissions.Any(e => e.PermissionID == id);
        }
    }
}
