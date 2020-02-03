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
    public class ReservationsController : AbstractPolicyController
    {
        public ReservationsController(VetClinicContext context) : base(context) { }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.Reservations.Include(r => r.Patients).Include(r => r.ReservationAddedUser).Include(r => r.ReservationUpdatedUser).Include(r => r.ReservationUser);
            return View(await vetClinicContext.OrderByDescending(u => u.IsActive).ThenByDescending(u => u.UpdatedDate).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(DateTime DateOfVisit)
        {

            List<User> userList = new List<User>();
            userList = _context.Users.ToList();
            //userList.Insert(0, new User { UserID = 0, FirstName = "Wybierz" });            
            userList.Insert(0, new User {  FirstName = "Wybierz" });
            ViewBag.ReservationUserID = new SelectList(userList, "UserID", "Fullname");
            ViewData["DateOfVisit"] = DateOfVisit;

            return View("Create");
        }
        public JsonResult GetPatient(int UserID)
        {
            List<Patient> patientsList = new List<Patient>();
            patientsList = _context.Patients.Where(x => x.PatientUserID == UserID).ToList();
            //patientsList.Insert(0, new Patient { PatientID = 0, Name = "Wybierz Pacjenta" });
            return Json(new SelectList(patientsList, "PatientID", "Name"));
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime DateOfVisitFromCalendar, [Bind("ReservationID,ReservationUserID,PatientID,Description")] Data.Data.Clinic.Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
                reservation.AddedDate = DateTime.Now;
                reservation.UpdatedDate = DateTime.Now;
                reservation.IsActive = true;
                reservation.AddedUserID = UserId;
                reservation.DateOfVisit = DateOfVisitFromCalendar;
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Calendar");
            }
            return Content("cos poszlo nie tak");
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            var userReservationId = _context.Reservations.Where(r => r.ReservationID == id).Select(u => u.ReservationUserID).FirstOrDefault();
            var patientList = _context.Patients.Where(p => p.PatientUserID == userReservationId).ToList();
            var patientId = _context.Reservations.Where(r => r.ReservationID == id).Select(p => p.PatientID).FirstOrDefault();

            ViewData["PatientID"] = new SelectList(patientList, "PatientID", "Name", patientId);
            ViewData["ReservationUserID"] = new SelectList(_context.Users, "UserID", "LastName", reservation.ReservationUserID);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationID,ReservationUserID,PatientID,Description,DateOfVisit,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Reservation reservation)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
                    reservation.UpdatedUserID = UserId;
                    reservation.UpdatedDate = DateTime.Now;
                    reservation.IsActive = true;
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationID))
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
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name", reservation.PatientID);
            ViewData["ReservationUserID"] = new SelectList(_context.Users, "UserID", "LastName", reservation.ReservationUserID);

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            reservation.IsActive = false;
            reservation.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            reservation.IsActive = true;
            reservation.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationID == id);
        }


        public async Task<IActionResult> ShowOwnReservation(string searchString)
        {

            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                int UserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                var data = DateTime.Today;
                List<int> listreservationsID = _context.Reservations.Where(v=>v.DateOfVisit>data).Where(v => v.DateOfVisit<data.AddDays(1)).Select(p => p.ReservationID).ToList();
                var ownReservations = _context.Reservations.Include(p => p.Patients).Include(p=>p.ReservationUser).Where(v => listreservationsID.Contains(v.ReservationID)).Where(r=>r.VisitID==null);

                ViewData["CurrentFilter"] = searchString;
                ViewData["PatientUserID"] = new SelectList(_context.PatientTypes, "PatientUserID", "Name");

                if (!String.IsNullOrEmpty(searchString))
                {
                    ownReservations = (from order in ownReservations
                                   where order.Patients.Name.Contains(searchString) || order.Patients.PatientNumber.Contains(searchString)
                                                                                || order.Patients.PatientUser.FirstName.Contains(searchString)
                                                                                || order.Patients.PatientUser.LastName.Contains(searchString)
                                                                                || order.Patients.PatientType.Name.Contains(searchString)
                                   select order)
                                        .Include(m => m.Patients.PatientUser)
                                        .Include(m => m.Patients.PatientType)
                                        .Include(m => m.Patients.Visits);

                }

                return View(await ownReservations.OrderBy(u => u.DateOfVisit).ToListAsync());

            }
            return View();
        }

        public IActionResult AddVisitFromReservation(int id)
        {
            ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "LastName");
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name");
            ViewData["VetName"] = new SelectList(_context.Users.Where(u => u.UserTypeID == 2)/*2 to id lekarzy*/, "UserID", "LastName");
            ViewData["VetID"] = new SelectList(_context.Users.Where(u => u.UserTypeID == 2)/*2 to id lekarzy*/, "UserID", "UserID");
            ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name");
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "LastName");
            ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "LastName");
            ViewData["MedicineID"] = new SelectList(_context.Medicines, "MedicineID", "Name");
            ViewData["CzyLekarz"] = _context.Users.Where(a => a.UserID == Int32.Parse(HttpContext.Session.GetString("UserID")))
                .Select(a => a.UserTypeID).FirstOrDefault() == 2;//typeid dla lekarza
            ViewData["ZalogowanyLekarz"] = _context.Users.FirstOrDefault(a => a.UserID == Int32.Parse(HttpContext.Session.GetString("UserID"))).Fullname;

            var visit = _context.Reservations.FirstOrDefault(p => p.ReservationID == id);
            return View(visit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVisitFromReservation(int? id, [Bind("VisitID,VisitUserID,PatientID,VetID,TreatmentID,DateOfVisit,Description,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Visit visit
                    , [Bind("PatientID")]Patient patient, Reservation reservation, int id2,int id3)
        {
            // ViewData["MedicineID"] = new MultiSelectList(_context.Medicines, "MedicineID", "Name",medicinesID);
            id = patient.PatientID;
            id2 = reservation.ReservationID;


            //if (ModelState.IsValid)
            //{
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
            id3 = visit.VisitID;
            var res = await _context.Reservations.FindAsync(id2);
            res.VisitID = id3;
            await _context.SaveChangesAsync();
            return RedirectToAction("Visit", "Patients", new { id = visit.PatientID }, Convert.ToString(visit.VisitID));
            //}
            //ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "LastName", visit.VisitUserID);
            //ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name", visit.PatientID);
            //ViewData["VetID"] = new SelectList(_context.Users.Where(u => u.UserTypeID == 2)/*2 to id lekarzy*/, "UserID", "LastName", visit.VetID);
            //ViewData["TreatmentID"] = new SelectList(_context.Treatments, "TreatmentID", "Name",visit.TreatmentID);
            //ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "LastName", visit.AddedUserID);
            //ViewData["VisitUserID"] = new SelectList(_context.Users, "UserID", "LastName", visit.UpdatedUserID);
            //return View(visit);
        }
    }
}
