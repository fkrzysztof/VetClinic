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
    public class MedicalSpecializationsController : AbstractPolicyController
    {
        public MedicalSpecializationsController(VetClinicContext context) :base(context) { }

        // GET: MedicalSpecialization
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.MedicalSpecializations.Include(m => m.MedicalSpecializationAddedUser).Include(m => m.MedicalSpecializationUpdatedUser).Include(m => m.MedicalSpecializationUser).Include(m => m.Specialization);
            return View(await vetClinicContext.OrderByDescending(u => u.UpdatedDate).ToListAsync());
        }

        // GET: MedicalSpecialization/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(from user in _context.Users where user.IsActive == true where user.UserTypeID == 2 select new { user.UserID, Display_Name = user.FirstName + " " + user.LastName}, "UserID", "Display_Name");
            ViewData["SpecializationID"] = new SelectList(_context.Specializations.Where(s => s.IsActive == true), "SpecializationID", "Name");

            return View();
        }

        // POST: MedicalSpecialization/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicalSpecializationID,UserID,SpecializationID,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] MedicalSpecialization medicalSpecialization)
        {
            if (ModelState.IsValid)
            {
                int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
                medicalSpecialization.AddedUserID = UserId;
                medicalSpecialization.AddedDate = DateTime.Now;
                medicalSpecialization.IsActive = true;

                _context.Add(medicalSpecialization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(medicalSpecialization);
        }

        // GET: MedicalSpecialization/Edit/5
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

            ViewData["UserID"] = new SelectList(from user in _context.Users where user.IsActive == true where user.UserTypeID == 2 select new { user.UserID, Display_Name = user.FirstName + " " + user.LastName }, "UserID", "Display_Name", medicalSpecialization.UserID);
            ViewData["SpecializationID"] = new SelectList(_context.Specializations.Where(s => s.IsActive == true), "SpecializationID", "Name", medicalSpecialization.SpecializationID);

            return View(medicalSpecialization);
        }

        // POST: MedicalSpecialization/Edit/5
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
                    int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
                    medicalSpecialization.UpdatedUserID = UserId;
                    medicalSpecialization.UpdatedDate = DateTime.Now;
                    medicalSpecialization.IsActive = true;

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

            return View(medicalSpecialization);
        }

        // POST: MedicalSpecialization/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalSpecialization = await _context.MedicalSpecializations.FindAsync(id);
            medicalSpecialization.IsActive = false;
            medicalSpecialization.UpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var medicalSpecialization = await _context.MedicalSpecializations.FindAsync(id);
            medicalSpecialization.IsActive = true;

            medicalSpecialization.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MedicalSpecializationExists(int id)
        {
            return _context.MedicalSpecializations.Any(e => e.MedicalSpecializationID == id);
        }
        public IActionResult AddUserMedSpec(int id)
        {

            ViewData["UserID"] = new SelectList(from user in _context.Users where user.UserID == id where user.UserTypeID == 2 select new { user.UserID, Display_Name = user.FirstName + " " + user.LastName }, "UserID", "Display_Name");
            ViewData["SpecializationID"] = new SelectList(_context.Specializations.Where(s => s.IsActive == true), "SpecializationID", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserMedSpec(int id, [Bind("MedicalSpecializationID,UserID,SpecializationID,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] MedicalSpecialization medicalSpecialization)
        {

            medicalSpecialization.AddedDate = DateTime.Now;
            medicalSpecialization.UpdatedDate = medicalSpecialization.AddedDate;
            medicalSpecialization.IsActive = true;

            _context.Add(medicalSpecialization);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AddAnother));

        }
        public IActionResult AddAnother()
        {
            return View();
        }
        public async Task<IActionResult> DeleteUserMedSpec(int id)
        {
            var medicine = await _context.MedicalSpecializations.FirstOrDefaultAsync(a => a.MedicalSpecializationID == id);
            medicine.IsActive = false;           
            await _context.SaveChangesAsync();

            return View();
        }
    }
}
