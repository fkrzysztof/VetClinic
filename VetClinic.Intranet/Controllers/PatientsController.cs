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

namespace VetClinic.Intranet.Controllers
{
    public class PatientsController : Controller
    {
        private readonly VetClinicContext _context;
        private readonly int CustomerUserId = 4;

        public PatientsController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: Patient     
        public async Task<IActionResult> Index(string searchString)
        {

            ViewData["CurrentFilter"] = searchString;
            ViewData["PatientUserID"] = new SelectList(_context.PatientTypes, "PatientUserID", "Name");

            var vetClinicContext = _context.Patients.Include(m => m.PatientUser).Include(m => m.PatientType).Where(m => m.IsActive == true);
            if (!String.IsNullOrEmpty(searchString))
            {
                vetClinicContext = (from order in _context.Patients
                                    where order.Name.Contains(searchString) || order.PatientNumber.Contains(searchString)
                                                                            || order.PatientUser.FirstName.Contains(searchString)
                                                                            || order.PatientUser.LastName.Contains(searchString)
                                                                            || order.PatientType.Name.Contains(searchString)
                                    select order)
                                    .Include(m => m.PatientUser)
                                    .Include(m => m.PatientType)
                                    .Where(a => a.IsActive == true);

            }
            return View(await vetClinicContext.OrderByDescending(u => u.UpdatedDate).ToListAsync());
        }

        public async Task<IActionResult> ShowOwnPatients(string searchString)
        {
          
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                int UserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                IEnumerable<int?> listpatientsID = _context.Visits.Where(v => v.VetID == UserID).Select(p => p.PatientID).ToList();
                var ownPatients = _context.Patients.Include(p=>p.PatientType).Include(p=>p.PatientUser).Where(v => listpatientsID.Contains(v.PatientID));

                ViewData["CurrentFilter"] = searchString;
                ViewData["PatientUserID"] = new SelectList(_context.PatientTypes, "PatientUserID", "Name");

                if (!String.IsNullOrEmpty(searchString))
                {
                    ownPatients = (from order in ownPatients
                                   where order.Name.Contains(searchString) || order.PatientNumber.Contains(searchString)
                                                                                || order.PatientUser.FirstName.Contains(searchString)
                                                                                || order.PatientUser.LastName.Contains(searchString)
                                                                                || order.PatientType.Name.Contains(searchString)
                                        select order)
                                        .Include(m => m.PatientUser)
                                        .Include(m => m.PatientType)
                                        .Include(m=>m.Visits);

                }

                return View(await ownPatients.OrderByDescending(u => u.UpdatedDate).ToListAsync());

            }
            return View();     
        }

        public async Task<IActionResult> ChooseOwner (string searchString)
        {

            ViewData["CurrentFilter"] = searchString;
            var vetClinicContext = _context.Users.Include(u => u.UserType).Where(u => u.UserType.UserTypeID == CustomerUserId).Where(u => u.IsActive == true); 
            if (!String.IsNullOrEmpty(searchString))
            {
                vetClinicContext = (from user in vetClinicContext
                                    where user.Login.Contains(searchString)
                                    || user.LastName.Contains(searchString)
                                    || user.Email.Contains(searchString)
                                    || user.City.Contains(searchString)
                                    select user)
                                            .Include(m => m.UserType)
                                            .Where(m => m.IsActive == true);


            }
            return View(await vetClinicContext.OrderByDescending(u => u.UpdatedDate).ToListAsync());

        }
        public async Task<IActionResult> DetailsUpdated(int? id)
        {         
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
              .Include(p => p.PatientAddedUser)           
              .Include(p => p.PatientUpdatedUser)          
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
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "LastName");
            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "LastName");
            ViewData["PatientUserID"] = new SelectList(_context.Users, "UserID", "FirstName");
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
                patient.IsActive = true;
                patient.AddedDate = DateTime.Now;
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
                {
                    patient.AddedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                }
               
                _context.Add(patient);               
                await _context.SaveChangesAsync();
                string stala = "240";               
                patient.PatientNumber = stala + patient.PatientID;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "LastName", patient.AddedUserID);
            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name", patient.PatientTypeID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "LastName", patient.UpdatedUserID);
            ViewData["PatientUserID"] = new SelectList(_context.Users, "UserID", "FirstName", patient.PatientUserID);
            return View(patient);
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var patient = await _context.Patients.FindAsync(id);
            var patient = await _context.Patients.Include(p => p.PatientUser).FirstOrDefaultAsync(m => m.PatientID == id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "LastName", patient.AddedUserID);
            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name", patient.PatientTypeID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "LastName", patient.UpdatedUserID);
            ViewData["PatientUserID"] = new SelectList(_context.Users, "UserID", "LastName", patient.PatientUserID);
            return View(patient);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientID,PatientTypeID,PatientUserID,Name,BirthDate,PatientNumber,IsActive,Description,KennelName,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Patient patient)
        {
            if (id != patient.PatientID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    patient.UpdatedDate = DateTime.Now;
                    if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
                    {
                        patient.UpdatedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                    }
                    _context.Update(patient);
                    string stala = "240";
                    patient.PatientNumber = stala + patient.PatientID;
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
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "LastName", patient.AddedUserID);
            ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name", patient.PatientTypeID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "LastName", patient.UpdatedUserID);
            ViewData["PatientUserID"] = new SelectList(_context.Users, "UserID", "LastName", patient.PatientUserID);
            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            patient.IsActive = false;
            patient.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            patient.IsActive = true;
            patient.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientID == id);
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
            return View(await patient.AsNoTracking().ToListAsync());
        }

        public IActionResult AddVisit(int id)
        {


            ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "LastName");
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name");
            ViewData["VetID"] = new SelectList(_context.Users.Where(u => u.UserTypeID == 2)/*2 to id lekarzy*/, "UserID", "LastName");
            ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name");
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "LastName");
            ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "LastName");
            ViewData["MedicineID"] = new SelectList(_context.Medicines, "MedicineID", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVisit(int id, [Bind("VisitID,VisitUserID,PatientID,VetID,TreatmentID,DateOfVisit,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Visit visit)
        {
           // ViewData["MedicineID"] = new MultiSelectList(_context.Medicines, "MedicineID", "Name",medicinesID);
            if (ModelState.IsValid)
            {
                visit.VisitUserID = Int32.Parse(
                    _context.Patients.Where(p => p.PatientID == id).Select(a => a.PatientUserID).FirstOrDefault().ToString());
                visit.PatientID = id;
                //visit.VetID
                visit.AddedDate = DateTime.Now;
                visit.IsActive = true;
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
                {
                    visit.AddedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                }
                //do usuniecia jak baza bedize poprawiona
                visit.TreatmentID = _context.Treatments.Select(p => p.TreatmentID).First();
                _context.Add(visit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "LastName", visit.VisitUserID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name", visit.PatientID);
            ViewData["VetID"] = new SelectList(_context.Users.Where(u => u.UserTypeID == 2)/*2 to id lekarzy*/, "UserID", "LastName", visit.VetID);
            ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name",visit.TreatmentID);
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "LastName", visit.AddedUserID);
            ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "LastName", visit.UpdatedUserID);

            return View(visit);
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
                .Include(m=>m.Medicine)
                .Include(m=>m.Medicine.MedicineType)
                .Where(v => v.VisitID == id).ToList();
            var visitTreatments= _context.VisitTreatment
               .Include(m => m.Treatment)
               .Where(v => v.VisitID == id).ToList();
            var view = new VisitDetails
            {
                VisitTreatments= visitTreatments,
                VisitMedicine = visitMedicine,
                Visit = visit,
                //Patient = visit.Patient                
            };


            return View(view);
        }
        public async Task<IActionResult> EditVisit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
            if (view == null)
            {
                return NotFound();
            }
            ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "LastName");
            ViewData["VetID"] = new SelectList(from user in _context.Users where user.UserTypeID == 2 select new { user.UserID, Display_Name = user.FirstName + " " + user.LastName }, "UserID", "Display_Name");
            ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name");
            ViewData["MedicineID"] = new SelectList(_context.Medicines, "MedicineID", "Name");
            return View(view);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVisit(int id, 
            [Bind("VisitID,VisitUserID,PatientID,TreatmentID,DateOfVisit,IsActive,Description,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Visit visit,
            [Bind("MedicineID")] Medicine medicine, [Bind("TreatmentID")] Treatment treatment)
        {
            if (id != visit.VisitID)
            {
                return NotFound();
            }
                try
                {
                    visit.PatientID = Int32.Parse(
                    _context.Visits.Where(p => p.VisitID == id).Select(a => a.PatientID).FirstOrDefault().ToString());
                    visit.VisitUserID = Int32.Parse(
                    _context.Patients.Where(p => p.PatientID == visit.PatientID).Select(a => a.PatientUserID).FirstOrDefault().ToString());
                    //visit.VetID
                    visit.UpdatedDate = DateTime.Now;
                    if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
                    {
                        visit.UpdatedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                    }
                if (medicine.MedicineID != 0)
                {
                    var visitMedicine = new VisitMedicine();
                    visitMedicine.VisitID = id;
                    visitMedicine.MedicineID = medicine.MedicineID;
                    _context.Add(visitMedicine);
                }
                if (treatment.TreatmentID != 0)
                {
                    var visitTreatment = new VisitTreatment();
                    visitTreatment.VisitID = id;
                    visitTreatment.TreatmentID = treatment.TreatmentID;
                    _context.Add(visitTreatment);


                    
                }
                //do usuniecia jak baza bedize poprawiona
                visit.TreatmentID = _context.Treatments.Select(p=>p.TreatmentID).First();
                _context.Update(visit);
                   
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(visit.VisitID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Visit), new { @id = visit.PatientID });

        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("DeleteVisit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVisitConfirmed(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            visit.IsActive = false;
            visit.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Visit), new { id = visit.PatientID });
        }

        // POST: Admin/Restore/5
        [HttpPost, ActionName("RestoreVisit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreVisitConfirmed(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            visit.IsActive = true;
            visit.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Visit), new { id = visit.PatientID });
        }
    }
}
