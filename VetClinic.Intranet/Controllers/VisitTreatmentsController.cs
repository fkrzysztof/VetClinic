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
    public class VisitTreatmentsController : Controller
    {
        private readonly VetClinicContext _context;

        public VisitTreatmentsController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: VisitTreatments
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.VisitTreatment.Include(v => v.Treatment).Include(v => v.Visit);
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: VisitTreatments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitTreatment = await _context.VisitTreatment
                .Include(v => v.Treatment)
                .Include(v => v.Visit)
                .FirstOrDefaultAsync(m => m.VisitTreatmentID == id);
            if (visitTreatment == null)
            {
                return NotFound();
            }

            return View(visitTreatment);
        }

        // GET: VisitTreatments/Create
        public async Task<IActionResult> Create(int? id, int patientId)
        {

            var visitTreat = await _context.VisitTreatment.FirstOrDefaultAsync(m => m.Visit.VisitID == id);
            if (visitTreat == null)
            {
                visitTreat = new VisitTreatment { VisitID = id };
            }
            ViewData["TreatmentID"] = new SelectList(_context.Treatments.OrderBy(x => x.Name), "TreatmentID", "Name");
            ViewData["VisitID"] = new SelectList(_context.Visits, "VisitID", "VisitID");
            ViewBag.patientId = patientId;
            return View(visitTreat);
        }
        // POST: VisitTreatments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, int patientId, [Bind("VisitTreatmentID,VisitID,TreatmentID")] VisitTreatment visitTreatment)
        {
            id = visitTreatment.VisitID;
            if (ModelState.IsValid)
            {
                _context.Add(visitTreatment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Visit", "Patients", new { id = patientId });
            }
            ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name", visitTreatment.TreatmentID);
            ViewData["VisitID"] = new SelectList(_context.Visits, "VisitID", "VisitID", visitTreatment.VisitID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "PatientID");
            return View(visitTreatment);
        }

        // GET: VisitTreatments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitTreatment = await _context.VisitTreatment.FindAsync(id);
            if (visitTreatment == null)
            {
                return NotFound();
            }
            ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name", visitTreatment.TreatmentID);
            ViewData["VisitID"] = new SelectList(_context.Visits, "VisitID", "VisitID", visitTreatment.VisitID);
            return View(visitTreatment);
        }

        // POST: VisitTreatments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VisitTreatmentID,VisitID,TreatmentID")] VisitTreatment visitTreatment)
        {
            if (id != visitTreatment.VisitTreatmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visitTreatment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitTreatmentExists(visitTreatment.VisitTreatmentID))
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
            ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name", visitTreatment.TreatmentID);
            ViewData["VisitID"] = new SelectList(_context.Visits, "VisitID", "VisitID", visitTreatment.VisitID);
            return View(visitTreatment);
        }

        // GET: VisitTreatments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitTreatment = await _context.VisitTreatment
                .Include(v => v.Treatment)
                .Include(v => v.Visit)
                .FirstOrDefaultAsync(m => m.VisitTreatmentID == id);
            if (visitTreatment == null)
            {
                return NotFound();
            }

            return View(visitTreatment);
        }

        // POST: VisitTreatments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visitTreatment = await _context.VisitTreatment.FindAsync(id);
            _context.VisitTreatment.Remove(visitTreatment);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Patients");
        }

        private bool VisitTreatmentExists(int id)
        {
            return _context.VisitTreatment.Any(e => e.VisitTreatmentID == id);
        }

    }
}
