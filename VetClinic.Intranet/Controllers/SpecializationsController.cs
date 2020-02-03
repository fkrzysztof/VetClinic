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
    public class SpecializationsController : AbstractPolicyController
    {
        public SpecializationsController(VetClinicContext context) : base(context) { }

        // GET: Specialization
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.Specializations.Include(s => s.SpecializationAddedUser).Include(s => s.SpecializationUpdatedUser);
            return View(await vetClinicContext.OrderByDescending(u => u.UpdatedDate).ToListAsync());
        }

        
        // GET: Specialization/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specialization/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecializationID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Specialization specialization)
        {
            if (ModelState.IsValid)
            {
                int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
                specialization.AddedUserID = UserId;
                specialization.AddedDate = DateTime.Now;
                specialization.IsActive = true;

                _context.Add(specialization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(specialization);
        }

        // GET: Specialization/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialization = await _context.Specializations.FindAsync(id);
            if (specialization == null)
            {
                return NotFound();
            }

            return View(specialization);
        }

        // POST: Specialization/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpecializationID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Specialization specialization)
        {
            if (id != specialization.SpecializationID)
            {
                return NotFound();
            }

            specialization.UpdatedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
                    specialization.UpdatedUserID = UserId;
                    specialization.IsActive = true;
                    specialization.UpdatedDate = DateTime.Now;
                    _context.Update(specialization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecializationExists(specialization.SpecializationID))
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

            return View(specialization);
        }

        // POST: Specialization/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specialization = await _context.Specializations.FindAsync(id);
            specialization.IsActive = false;
            specialization.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var specialization = await _context.Specializations.FindAsync(id);
            specialization.IsActive = true;
            specialization.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool SpecializationExists(int id)
        {
            return _context.Specializations.Any(e => e.SpecializationID == id);
        }
    }
}
