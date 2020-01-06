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
using VetClinic.PortalWWW.Controllers.Abstract;

namespace VetClinic.PortalWWW.Controllers
{
    public class PatientController : BaseController
    {

        public PatientController(VetClinicContext context) : base(context) { }

        // GET: Patient
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserID"));
            var vetClinicContext = _context.Patients.Include(p => p.PatientAddedUser).Include(p => p.PatientType).Include(p => p.PatientUpdatedUser).Include(p => p.PatientUser).Where(u => u.PatientUserID == userId);
            return View(await vetClinicContext.ToListAsync());
        }

        public async Task<IActionResult> Leki(int? id)
        {

            var vetClinicContext = _context.Visits.Where(v => v.VisitID == id).Select(m => m.VisitMedicines);
            List<Medicine> medicines = new List<Medicine>();
            foreach(VisitMedicine med in vetClinicContext)
            {
                medicines.Add(med.Medicine);
            }

            return View(medicines);
        }

        // GET: Patient/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.PatientAddedUser)
                .Include(p => p.PatientType)
                .Include(p => p.PatientUpdatedUser)
                .Include(p => p.PatientUser)
                .FirstOrDefaultAsync(m => m.PatientID == id);
            if (patient == null)
            {
                return NotFound();
            }

            var wizyta = _context.Reservations.Where(r => r.PatientID == id);
           
            return View(await wizyta.ToListAsync());
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["PatientUserID"] = new SelectList(_context.Users, "UserID", "City");
            return View();
        }

        // POST: Patient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientID,PatientTypeID,PatientUserID,Name,BirthDate,PatientNumber,IsActive,Description,KennelName,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(HttpContext.Session.GetString("UserID"));
                patient.AddedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                patient.PatientUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                patient.UpdatedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                patient.IsActive = true;
                patient.AddedDate = DateTime.Now;

                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name", patient.PatientTypeID);         

            return View(patient);
        }

        // GET: Patient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.PatientAddedUser)
                .Include(p => p.PatientType)
                .Include(p => p.PatientUpdatedUser)
                .Include(p => p.PatientUser)
                .FirstOrDefaultAsync(m => m.PatientID == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            var reservation = _context.Reservations.Where(r => r.PatientID == id).ToList();
            foreach(Reservation res in reservation)
            {
                _context.Reservations.Remove(res);
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientID == id);
        }
    }
}
