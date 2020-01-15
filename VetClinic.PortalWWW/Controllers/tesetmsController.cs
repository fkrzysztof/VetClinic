using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.PortalWWW.Controllers
{
    public class tesetmsController : Controller
    {
        private readonly VetClinicContext _context;

        public tesetmsController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: tesetms
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.MedicalSpecializations.Include(m => m.MedicalSpecializationAddedUser).Include(m => m.MedicalSpecializationUpdatedUser).Include(m => m.MedicalSpecializationUser).Include(m => m.Specialization);
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: tesetms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalSpecialization = await _context.MedicalSpecializations
                .Include(m => m.MedicalSpecializationAddedUser)
                .Include(m => m.MedicalSpecializationUpdatedUser)
                .Include(m => m.MedicalSpecializationUser)
                .Include(m => m.Specialization)
                .FirstOrDefaultAsync(m => m.MedicalSpecializationID == id);
            if (medicalSpecialization == null)
            {
                return NotFound();
            }

            return View(medicalSpecialization);
        }

        // GET: tesetms/Create
        public IActionResult Create()
        {
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["SpecializationID"] = new SelectList(_context.Specializations, "SpecializationID", "Name");
            return View();
        }

        // POST: tesetms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicalSpecializationID,UserID,SpecializationID,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] MedicalSpecialization medicalSpecialization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicalSpecialization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", medicalSpecialization.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", medicalSpecialization.UpdatedUserID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "City", medicalSpecialization.UserID);
            ViewData["SpecializationID"] = new SelectList(_context.Specializations, "SpecializationID", "Name", medicalSpecialization.SpecializationID);
            return View(medicalSpecialization);
        }

        // GET: tesetms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalSpecialization = await _context.MedicalSpecializations.FindAsync(id);
            if (medicalSpecialization == null)
            {
                return NotFound();
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", medicalSpecialization.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", medicalSpecialization.UpdatedUserID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "City", medicalSpecialization.UserID);
            ViewData["SpecializationID"] = new SelectList(_context.Specializations, "SpecializationID", "Name", medicalSpecialization.SpecializationID);
            return View(medicalSpecialization);
        }

        // POST: tesetms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicalSpecializationID,UserID,SpecializationID,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] MedicalSpecialization medicalSpecialization)
        {
            if (id != medicalSpecialization.MedicalSpecializationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalSpecialization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalSpecializationExists(medicalSpecialization.MedicalSpecializationID))
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
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", medicalSpecialization.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", medicalSpecialization.UpdatedUserID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "City", medicalSpecialization.UserID);
            ViewData["SpecializationID"] = new SelectList(_context.Specializations, "SpecializationID", "Name", medicalSpecialization.SpecializationID);
            return View(medicalSpecialization);
        }

        // GET: tesetms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalSpecialization = await _context.MedicalSpecializations
                .Include(m => m.MedicalSpecializationAddedUser)
                .Include(m => m.MedicalSpecializationUpdatedUser)
                .Include(m => m.MedicalSpecializationUser)
                .Include(m => m.Specialization)
                .FirstOrDefaultAsync(m => m.MedicalSpecializationID == id);
            if (medicalSpecialization == null)
            {
                return NotFound();
            }

            return View(medicalSpecialization);
        }

        // POST: tesetms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalSpecialization = await _context.MedicalSpecializations.FindAsync(id);
            _context.MedicalSpecializations.Remove(medicalSpecialization);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalSpecializationExists(int id)
        {
            return _context.MedicalSpecializations.Any(e => e.MedicalSpecializationID == id);
        }
    }
}
