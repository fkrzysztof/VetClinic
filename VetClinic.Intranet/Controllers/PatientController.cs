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
    public class PatientController : Controller
    {
        private readonly VetClinicContext _context;

        public PatientController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: Patient
        //public async Task<IActionResult> Index()
        //{
        //    var vetClinicContext = _context.Patients.Include(p => p.PatientAddedUser).Include(p => p.PatientType).Include(p => p.PatientUpdatedUser).Include(p => p.PatientUser);
        //    return View(await vetClinicContext.ToListAsync());
        //}

        public async Task<IActionResult> Index(string searchString)
        {
            
            ViewData["CurrentFilter"] = searchString;

            var patients = from p in _context.Patients
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(p => p.Name.Contains(searchString)
                                       || p.Description.Contains(searchString)
                                       || p.BirthDate.ToString().Contains(searchString));
            }
            
            return View(await patients.AsNoTracking().ToListAsync());
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

            return View(patient);
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
        public async Task<IActionResult> Create([Bind("PatientID,PatientTypeID,PatientUserID,Name,BirthDate,PatientNumber,IsActive,Description,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", patient.AddedUserID);
            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name", patient.PatientTypeID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", patient.UpdatedUserID);
            ViewData["PatientUserID"] = new SelectList(_context.Users, "UserID", "City", patient.PatientUserID);
            return View(patient);
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", patient.AddedUserID);
            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name", patient.PatientTypeID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", patient.UpdatedUserID);
            ViewData["PatientUserID"] = new SelectList(_context.Users, "UserID", "City", patient.PatientUserID);
            return View(patient);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientID,PatientTypeID,PatientUserID,Name,BirthDate,PatientNumber,IsActive,Description,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Patient patient)
        {
            if (id != patient.PatientID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.PatientID))
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
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", patient.AddedUserID);
            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name", patient.PatientTypeID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", patient.UpdatedUserID);
            ViewData["PatientUserID"] = new SelectList(_context.Users, "UserID", "City", patient.PatientUserID);
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
