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
using VetClinic.Data.Helpers;
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
        public async Task<IActionResult> Visit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = _context.Visits.Where(p => p.PatientID == id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["PatientName"] = _context.Patients.Where(p => p.PatientID == id).Select(a => a.Name).FirstOrDefault();
            ViewData["PatientID"] = id;
            ViewData["PatientUserFirstName"] = _context.Patients.Where(p => p.PatientID == id).Select(a => a.PatientUser.FirstName).FirstOrDefault();
            ViewData["PatientUserLastName"] = _context.Patients.Where(p => p.PatientID == id).Select(a => a.PatientUser.LastName).FirstOrDefault();

            ViewBag.PatientName = _context.Patients.Where(p => p.PatientID == id).Select(name => name.Name).FirstOrDefault();
            ViewBag.BirthDate = _context.Patients.Where(p => p.PatientID == id).Select(date => date.BirthDate).FirstOrDefault();
            ViewBag.PatientUserLastname = _context.Patients.Where(p => p.PatientID == id).Select(ownerName => ownerName.PatientUser.LastName).FirstOrDefault();
            return View(await patient.AsNoTracking().ToListAsync());
        }

        public int DeleteVisitTretment(int TreatmentID, int VisitID)
        {
            VisitTreatment visitTreatments = _context.VisitTreatment
               .Where(t => t.TreatmentID == TreatmentID)
               .Where(v => v.VisitID == VisitID).FirstOrDefault();

            _context.VisitTreatment.Remove(visitTreatments);
            _context.SaveChangesAsync();
            return VisitID;
        }
        public int DeleteVisitMedicine(int MedicineID, int VisitID)
        {
            VisitMedicine visitMedicine = _context.VisitMedicines
               .Where(t => t.MedicineID == MedicineID)
               .Where(v => v.VisitID == VisitID).FirstOrDefault();

            _context.VisitMedicines.Remove(visitMedicine);
            _context.SaveChangesAsync();
            return VisitID;
        }

        public IActionResult VisitDetails(int id)
        {


            var visit = _context.Visits
                   .Include(v => v.Patient)
                   .Include(v => v.VetUser)
                   .Include(v => v.VisitUser)
                   .Include(v => v.Treatment)
                   .Include(v => v.VisitMedicines)
                   .Where(v => v.VisitID == id)
                   .FirstOrDefault();

            var visitMedicine = _context.VisitMedicines
                .Include(m => m.Medicine)
                .Include(m => m.Medicine.MedicineType)
                .Where(v => v.VisitID == id).ToList();
            var visitTreatments = _context.VisitTreatment
               .Include(m => m.Treatment)
               .Where(v => v.VisitID == id).ToList();
            var view = new VisitDetails
            {
                VisitTreatments = visitTreatments,
                VisitMedicine = visitMedicine,
                Visit = visit,
                //Patient = visit.Patient                
            };


            return View(view);
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

            //var wizyta = _context.Reservations.Where(r => r.PatientID == id);
           
            //return View(await wizyta.ToListAsync());
            return View(patient);
        }

        // GET: Patient/Create
        public IActionResult Create(DateTime? DateOfVisit)
        {

            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["PatientUserID"] = new SelectList(_context.Users, "UserID", "City");
            if (DateOfVisit > DateTime.MinValue)
            {
                ViewData["DateOfVisit"] = DateOfVisit;

            }
            return View();
        }

        // POST: Patient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime? DateOfVisit,[Bind("PatientID,PatientTypeID,PatientUserID,Name,BirthDate,PatientNumber,IsActive,Description,KennelName,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Patient patient)
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
                if (DateOfVisit > DateTime.MinValue)
                {
                    TempData["DateOfVisit"] = DateOfVisit;
                    return RedirectToAction("Create","Reservation", new { PatientId = patient.PatientID });
                }

                return RedirectToAction(nameof(Index));
            }
           
            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name", patient.PatientTypeID);         

            return View(patient);
        }

        public IActionResult CreatePatientFromReservation(DateTime Date)
        {
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["PatientUserID"] = new SelectList(_context.Users, "UserID", "City");
            var a = Date;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePatientFromReservation([Bind("PatientID,PatientTypeID,PatientUserID,Name,BirthDate,PatientNumber,IsActive,Description,KennelName,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Patient patient, DateTime DateOfVisit)
        {
            var a = DateOfVisit;
            if (ModelState.IsValid)
            {
                int id = int.Parse(HttpContext.Session.GetString("UserID"));
                patient.AddedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                patient.PatientUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                patient.UpdatedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                patient.IsActive = true;
                patient.AddedDate = DateTime.Now;

                //_context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Reservation");
            }

            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name", patient.PatientTypeID);

            return RedirectToAction("Create", "Reservation", DateOfVisit);
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
            var visitList = _context.Visits.Where(p => p.PatientID == id).ToList();
            foreach (Visit visit in visitList)
            {
                visit.PatientID = null;
                _context.Visits.Update(visit);
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
