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

        public PatientsController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: Patient     
        public async Task<IActionResult> Index(string searchString)
        {

            ViewData["CurrentFilter"] = searchString;
            ViewData["PatientUserID"] = new SelectList(_context.PatientTypes, "PatientUserID", "Name");

            var vetClinicContext = _context.Patients.Include(m => m.PatientUser).Include(m => m.PatientType);
            if (!String.IsNullOrEmpty(searchString))
            {
                vetClinicContext = (from order in _context.Patients
                                    where order.Name.Contains(searchString) || order.PatientNumber.Contains(searchString)
                                                                            || order.PatientUser.FirstName.Contains(searchString)
                                                                            || order.PatientUser.LastName.Contains(searchString)
                                                                            || order.PatientType.Name.Contains(searchString)
                                    select order)
                                    .Include(m => m.PatientUser)
                                    .Include(m => m.PatientType);

            }
            return View(await vetClinicContext.ToListAsync());
        }

        public async Task<IActionResult> PokazMoje(string searchString)
        {
          
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                int UserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                IEnumerable<int?> listpatientsID = _context.Visits.Where(v => v.VetID == UserID).Select(p => p.PatientID).ToList();
                var listaWlasnych = _context.Patients.Include(p=>p.PatientType).Include(p=>p.PatientUser).Where(v => listpatientsID.Contains(v.PatientID));

                ViewData["CurrentFilter"] = searchString;
                ViewData["PatientUserID"] = new SelectList(_context.PatientTypes, "PatientUserID", "Name");

                if (!String.IsNullOrEmpty(searchString))
                {
                    listaWlasnych = (from order in listaWlasnych
                                        where order.Name.Contains(searchString) || order.PatientNumber.Contains(searchString)
                                                                                || order.PatientUser.FirstName.Contains(searchString)
                                                                                || order.PatientUser.LastName.Contains(searchString)
                                                                                || order.PatientType.Name.Contains(searchString)
                                        select order)
                                        .Include(m => m.PatientUser)
                                        .Include(m => m.PatientType)
                                        .Include(m=>m.Visits);

                }

                return View(await listaWlasnych.ToListAsync());

            }
            return View();
                                 
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
        public async Task<IActionResult> Create([Bind("PatientID,PatientTypeID,PatientUserID,Name,BirthDate,PatientNumber,IsActive,Description,KennelName,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Patient patient)
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
            //ViewData["PatientID"] = (from p in _context.Patients
            //                         where p.PatientID==id
            //                        select new{
            //                            p.Name,
            //                            p.PatientUserID
            //                                    }).FirstOrDefault();
            ViewData["PatientName"] = _context.Patients.Where(p => p.PatientID == id).Select(a => a.Name).FirstOrDefault();
            ViewData["PatientID"] = id;
            //ViewData["PatientTypeID"] = new SelectList(_context.PatientTypes, "PatientTypeID", "Name", patient.PatientTypeID);
            //ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", patient.UpdatedUserID);
            //ViewData["PatientUserID"] = new SelectList(_context.Users, "UserID", "City", patient.PatientUserID);
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
                //visit.AddedUserID
                //do usuniecia jak baza bedize poprawiona
                visit.TreatmentID = 1;
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
        
        public IActionResult AddMedicine(int id)
        {

            ViewData["VetID"] = new SelectList(_context.Users.Where(u => u.UserTypeID == 2)/*2 to id lekarzy*/, "UserID", "LastName");
            //tu pewnie trzeba przekazac typ leku
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMedicine(int id, [Bind("VisitMedicineID,VisitID,MedicineID,MedicineTypeID,Name,Description,Price,Quantity,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] VisitMedicine visitmedicine
                                                                )
        {

            if (ModelState.IsValid)
            {
                //visit.VisitUserID
                visitmedicine.VisitID = id;
                //visit.VetID
                visitmedicine.Medicine.AddedDate = DateTime.Now;
                visitmedicine.Medicine.IsActive = true;
                //visit.AddedUserID
                _context.Add(visitmedicine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MedicineTypeID"] = new SelectList(_context.Medicines, "MedicineTypeID", "Name", visitmedicine.Medicine.MedicineTypeID);

            return View(visitmedicine);
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

            var visits = await _context.Visits.FindAsync(id);
            var visit = new VisitDetails();
            visit.Visit = visits;

            //var operations = await _context.Operations.ToListAsync();
            if (visit == null)
            {
                return NotFound();
            }
            //var visitDetails = (from v in _context.Visits
            //                    where v.VisitID == id
            //                    join o in operations on v.VisitID equals o.VisitID into tbl1
            //                   from o in tbl1.ToList()
            //                   select new VisitDetails
            //                   {
            //                       visit=v,
            //                       operation=o
            //                   }).ToList();
            ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "LastName");
            ViewData["VetID"] = new SelectList(_context.Users.Where(u => u.UserTypeID == 2)/*2 to id lekarzy*/, "UserID", "LastName");
            ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name");
            ViewData["MedicineID"] = new SelectList(_context.Medicines, "MedicineID", "Name");
            return View(visit);
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
                //visit.IsActive = true;
                //visit.AddedUserID
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
                visit.TreatmentID = 1;
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
    }
}
