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
    public class VisitMedicinesController : Controller
    {
        private readonly VetClinicContext _context;

        public VisitMedicinesController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: VisitMedicines
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.VisitMedicines.Include(v => v.Medicine).Include(v => v.Visit);
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: VisitMedicines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitMedicine = await _context.VisitMedicines
                .Include(v => v.Medicine)
                .Include(v => v.Visit)
                .FirstOrDefaultAsync(m => m.VisitMedicineID == id);
            if (visitMedicine == null)
            {
                return NotFound();
            }

            return View(visitMedicine);
        }

        // GET: VisitMedicines/Create
        public IActionResult Create()
        {
            ViewData["MedicineID"] = new SelectList(_context.Medicines, "MedicineID", "Name");
            ViewData["VisitID"] = new SelectList(_context.Visits, "VisitID", "VisitID");
            return View();
        }

        // POST: VisitMedicines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VisitMedicineID,VisitID,MedicineID")] VisitMedicine visitMedicine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visitMedicine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicineID"] = new SelectList(_context.Medicines, "MedicineID", "Name", visitMedicine.MedicineID);
            ViewData["VisitID"] = new SelectList(_context.Visits, "VisitID", "VisitID", visitMedicine.VisitID);
            return View(visitMedicine);
        }

        // GET: VisitMedicines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitMedicine = await _context.VisitMedicines.FindAsync(id);
            if (visitMedicine == null)
            {
                return NotFound();
            }
            ViewData["MedicineID"] = new SelectList(_context.Medicines, "MedicineID", "Name", visitMedicine.MedicineID);
            ViewData["VisitID"] = new SelectList(_context.Visits, "VisitID", "VisitID", visitMedicine.VisitID);
            return View(visitMedicine);
        }

        // POST: VisitMedicines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VisitMedicineID,VisitID,MedicineID")] VisitMedicine visitMedicine)
        {
            if (id != visitMedicine.VisitMedicineID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visitMedicine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitMedicineExists(visitMedicine.VisitMedicineID))
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
            ViewData["MedicineID"] = new SelectList(_context.Medicines, "MedicineID", "Name", visitMedicine.MedicineID);
            ViewData["VisitID"] = new SelectList(_context.Visits, "VisitID", "VisitID", visitMedicine.VisitID);
            return View(visitMedicine);
        }

        // GET: VisitMedicines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitMedicine = await _context.VisitMedicines
                .Include(v => v.Medicine)
                .Include(v => v.Visit)
                .FirstOrDefaultAsync(m => m.VisitMedicineID == id);
            if (visitMedicine == null)
            {
                return NotFound();
            }

            return View(visitMedicine);
        }

        // POST: VisitMedicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visitMedicine = await _context.VisitMedicines.FindAsync(id);
            _context.VisitMedicines.Remove(visitMedicine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitMedicineExists(int id)
        {
            return _context.VisitMedicines.Any(e => e.VisitMedicineID == id);
        }
    }
}
