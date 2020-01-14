using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Intranet.Controllers.Abstract;

namespace VetClinic.Intranet.Controllers
{
    public class PatientTypesController : AbstractPolicyController
    {
        public PatientTypesController(VetClinicContext context) : base(context) { }

        // GET: PatientTypes
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.PatientTypes.Include(m => m.PatientTypeAddedUser).Include(m => m.PatientTypeUpdatedUser);
            return View(await vetClinicContext.OrderByDescending(u => u.UpdatedDate).ToListAsync());
        }

        // GET: PatientTypes/Create
        public IActionResult Create()
        {
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            return View();
        }

        // POST: PatientTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientTypeID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] PatientType patientType)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
                {
                    patientType.AddedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                }
                patientType.IsActive = true;
                patientType.AddedDate = DateTime.Now;
                _context.Add(patientType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", patientType.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", patientType.UpdatedUserID);
            return View(patientType);
        }

        // GET: PatientTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientType = await _context.PatientTypes.FindAsync(id);
            if (patientType == null)
            {
                return NotFound();
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", patientType.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", patientType.UpdatedUserID);
            return View(patientType);
        }

        // POST: PatientTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientTypeID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] PatientType patientType)
        {
            if (id != patientType.PatientTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
                    {
                        patientType.UpdatedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                    }
                    patientType.UpdatedDate = DateTime.Now;
                    patientType.IsActive = true;
                    _context.Update(patientType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientTypeExists(patientType.PatientTypeID))
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
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", patientType.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", patientType.UpdatedUserID);
            return View(patientType);
        }
        // POST: PatientTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientType = await _context.PatientTypes.FindAsync(id);
            patientType.IsActive = false;
            patientType.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var patientType = await _context.PatientTypes.FindAsync(id);
            patientType.IsActive = true;
            patientType.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PatientTypeExists(int id)
        {
            return _context.PatientTypes.Any(e => e.PatientTypeID == id);
        }
    }
}
