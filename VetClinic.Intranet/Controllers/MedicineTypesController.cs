﻿using System;
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
    public class MedicineTypesController : Controller
    {
        private readonly VetClinicContext _context;

        public MedicineTypesController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: MedicineTypes
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.MedicineTypes.Include(m => m.MedicineTypeAddedUser).Include(m => m.MedicineTypeUpdatedUser);
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: MedicineTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineType = await _context.MedicineTypes
                .Include(m => m.MedicineTypeAddedUser)
                .Include(m => m.MedicineTypeUpdatedUser)
                .FirstOrDefaultAsync(m => m.MedicineTypeID == id);
            if (medicineType == null)
            {
                return NotFound();
            }

            return View(medicineType);
        }

        // GET: MedicineTypes/Create
        public IActionResult Create()
        {
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            return View();
        }

        // POST: MedicineTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicineTypeID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] MedicineType medicineType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicineType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", medicineType.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", medicineType.UpdatedUserID);
            return View(medicineType);
        }

        // GET: MedicineTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineType = await _context.MedicineTypes.FindAsync(id);
            if (medicineType == null)
            {
                return NotFound();
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", medicineType.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", medicineType.UpdatedUserID);
            return View(medicineType);
        }

        // POST: MedicineTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicineTypeID,Name,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] MedicineType medicineType)
        {
            if (id != medicineType.MedicineTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicineType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicineTypeExists(medicineType.MedicineTypeID))
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
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", medicineType.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", medicineType.UpdatedUserID);
            return View(medicineType);
        }

        // GET: MedicineTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicineType = await _context.MedicineTypes
                .Include(m => m.MedicineTypeAddedUser)
                .Include(m => m.MedicineTypeUpdatedUser)
                .FirstOrDefaultAsync(m => m.MedicineTypeID == id);
            if (medicineType == null)
            {
                return NotFound();
            }

            return View(medicineType);
        }

        // POST: MedicineTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicineType = await _context.MedicineTypes.FindAsync(id);
            _context.MedicineTypes.Remove(medicineType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicineTypeExists(int id)
        {
            return _context.MedicineTypes.Any(e => e.MedicineTypeID == id);
        }
    }
}