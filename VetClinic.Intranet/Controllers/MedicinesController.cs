using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Intranet.Controllers
{
    public class MedicinesController : Controller
    {
        private readonly VetClinicContext _context;

        public MedicinesController(VetClinicContext context)
        {
            _context = context;
        }
        
        // GET: Medicines
        public async Task<IActionResult> Index(string searchString)
        {
            var polishFormat = new CultureInfo("pl-PL");
            ViewData["CurrentFilter"] = searchString;
            ViewData["MedicineTypeID"] = new SelectList(_context.MedicineTypes, "MedicineTypeID", "Name");
            var vetClinicContext = _context.Medicines.Include(m => m.MedicineAddedUser).Include(m => m.MedicineType).Include(m => m.MedicineUpdatedUser).Where(a=>a.IsActive==true);
            if (!String.IsNullOrEmpty(searchString))
            {
                vetClinicContext = (from order in _context.Medicines
                                    where order.Name.Contains(searchString) 
                                    || order.MedicineType.Name.Contains(searchString)
                                    select order)
                                 .Include(m => m.MedicineAddedUser)
                                 .Include(m => m.MedicineType)
                                 .Include(m => m.MedicineUpdatedUser)
                                 .Where(a => a.IsActive == true);
            }
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: Medicines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines
                .Include(m => m.MedicineAddedUser)
                .Include(m => m.MedicineType)
                .Include(m => m.MedicineUpdatedUser)
                .FirstOrDefaultAsync(m => m.MedicineID == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // GET: Medicines/Create
        public IActionResult Create()
        {
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["MedicineTypeID"] = new SelectList(_context.MedicineTypes, "MedicineTypeID", "Name");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            return View();
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicineID,MedicineTypeID,Name,Description,Price,Quantity,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                medicine.IsActive = true;
                medicine.AddedDate = DateTime.Now;
                //dodac urzytkownika
                //medicine.AddedUserID = current;
                _context.Add(medicine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", medicine.AddedUserID);
            ViewData["MedicineTypeID"] = new SelectList(_context.MedicineTypes, "MedicineTypeID", "Name", medicine.MedicineTypeID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", medicine.UpdatedUserID);
            return View(medicine);
        }

        // GET: Medicines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", medicine.AddedUserID);
            ViewData["MedicineTypeID"] = new SelectList(_context.MedicineTypes, "MedicineTypeID", "Name", medicine.MedicineTypeID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", medicine.UpdatedUserID);
            return View(medicine);
        }

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicineID,MedicineTypeID,Name,Description,Price,Quantity,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Medicine medicine)
        {
            if (id != medicine.MedicineID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    medicine.UpdatedDate = DateTime.Now;
                    //dodac urzytkownika
                    _context.Update(medicine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicineExists(medicine.MedicineID))
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
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", medicine.AddedUserID);
            ViewData["MedicineTypeID"] = new SelectList(_context.MedicineTypes, "MedicineTypeID", "Name", medicine.MedicineTypeID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", medicine.UpdatedUserID);
            return View(medicine);
        }

        // GET: Medicines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicines
                .Include(m => m.MedicineAddedUser)
                .Include(m => m.MedicineType)
                .Include(m => m.MedicineUpdatedUser)
                .FirstOrDefaultAsync(m => m.MedicineID == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            medicine.IsActive=false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.MedicineID == id);
        }
    }
}
