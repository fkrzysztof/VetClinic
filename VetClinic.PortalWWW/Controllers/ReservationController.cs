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
    public class ReservationController : BaseController
    {

        public ReservationController(VetClinicContext context) : base(context) { }

        //GET: Reservation
        public async Task<IActionResult> Index()
        {
            int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
            var vetClinicContext = _context.Reservations.Where(r => r.ReservationUserID == UserId).Include(r => r.Patients).Include(r => r.ReservationAddedUser).Include(r => r.ReservationUpdatedUser).Include(r => r.ReservationUser);

            //MCZ: wyświetla na widoku rezerwacji NAJBLIŻSZĄ wizytę, która jeszcze nie minęla
            string reservationDate =
                                (
                                from data in vetClinicContext
                                where data.DateOfVisit >= DateTime.Now.AddHours(1) //MCZ: z uwagi na to, że system serwera ma -1 godzinę to musiałam dodać jedną godzinę. 
                                && data.ReservationUserID == UserId
                                && data.IsActive == true
                                orderby data.DateOfVisit
                                select data.DateOfVisit
                                ).FirstOrDefault().ToString();

            ViewData["ClosestVisit"] = (((EmptyIfNullDate(reservationDate)).Replace("/", ".")).Replace(@"\", ".")).Replace("-",".");

            //MCZ: wyświetla na widoku rezerwacji pacjenta na NAJBLIŻSZĄ wizytę, która jeszcze nie minęla
            var patientId =
                        (
                        from data in _context.Reservations
                        where data.DateOfVisit >= DateTime.Now.AddHours(1) //MCZ: z uwagi na to, że system serwera ma -1 godzinę to musiałam dodać jedną godzinę. 
                        && data.ReservationUserID == UserId
                        && data.IsActive == true
                        orderby data.DateOfVisit
                        select data.PatientID
                        ).FirstOrDefault();

            if (patientId != null)
                {
                    ViewData["ClosestVisitPatient"] =
                                    (
                                    from data in _context.Reservations
                                    where data.DateOfVisit >= DateTime.Now.AddHours(1) 
                                    && data.ReservationUserID == UserId
                                    && data.IsActive == true
                                    orderby data.DateOfVisit
                                    select data.Patients.Name
                                    ).FirstOrDefault().ToString();

                    ViewData["WelcomeMessage"] = "Zapraszamy !";
                }
                else
                    {
                        ViewData["ClosestVisitPatient"] = "brak danych";
                        ViewData["WelcomeMessage"] = "";
                    }

            return View(await vetClinicContext.Where(x => x.DateOfVisit >= DateTime.Now.AddHours(1)).Where(x => x.IsActive == true).OrderBy(u => u.DateOfVisit).ToListAsync());
            //^ MCZ: wyświetlanie od najświeższej wizyty, o ile wizyta nie jest w czasie przeszłym i o ile jest AKTYWNA
        }

        //MCZ: Date - in case of empty reservation
        public string EmptyIfNullDate(string value)
        {
            if (String.IsNullOrEmpty(value))
                return "brak zarezerwowanej wizyty!";
            if (value.Substring(0, value.Length - 9) == "01.01.0001")
                return value = "brak zarezerwowanej wizyty!";
            if (value.Substring(0, value.Length - 9) == @"01\01\0001")
                return value = "brak zarezerwowanej wizyty!";
            if (value.Substring(0, value.Length - 9) == @"01/01/0001")
                return value = "brak zarezerwowanej wizyty!";
            if (value.Substring(0, value.Length - 9) == "0001-01-01")
                return value = "brak zarezerwowanej wizyty!";
            if (value.Substring(0, value.Length - 9) == @"0001\01\01")
                return value = "brak zarezerwowanej wizyty!";
            if (value.Substring(0, value.Length - 9) == @"0001/01/01")
                return value = "brak zarezerwowanej wizyty!";
            if (value.Substring(0, value.Length - 9) == "0001.01.01")
                return value = "brak zarezerwowanej wizyty!";
            else
                value = value.Substring(0, value.Length - 3);
            return value;
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Patients)
                .Include(r => r.ReservationAddedUser)
                .Include(r => r.ReservationUpdatedUser)
                .Include(r => r.ReservationUser)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(DateTime DateOfVisit)
        {
            int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
            ViewData["PatientID"] = new SelectList(_context.Patients.Where(p => p.PatientUserID == UserId), "PatientID", "Name");        
            ViewData["DateOfVisit"] = DateOfVisit;

            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime DateOfVisitFromCalendar, [Bind("ReservationID,ReservationUserID,PatientID,Description")] Data.Data.Clinic.Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
                {
                    reservation.AddedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                    reservation.ReservationUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                }

                reservation.DateOfVisit = DateOfVisitFromCalendar;
                reservation.AddedDate = DateTime.Now;
                reservation.IsActive = true;
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ClientPanel");
            }

            ViewData["PatientID"] = new SelectList(_context.Reservations, "Name", "PatientID", reservation.PatientID);

            return Content("cos poszlo nie tak");
       
        }

        //GET: Reservation/Create
        public IActionResult Create(int? PatientId)
        {
            int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
            if(PatientId!=null)
                ViewData["PatientID"] = new SelectList(_context.Patients.Where(p => p.PatientUserID == UserId), "PatientID", "Name", PatientId);
            ViewData["PatientID"] = new SelectList(_context.Patients.Where(p=>p.PatientUserID == UserId), "PatientID", "Name");
            //ViewData["BlokCzasowy"] = new SelectList(_context.ScheduleBlocks.OrderBy(b => b.Time), "ScheduleBlockID", "Time");
            return View();
        }



       

        // GET: Reservation/Edit/5
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

            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
                ViewData["PatientID"] = new SelectList(_context.Patients.
                        Where(p => p.PatientUserID != null).
                        Where(p => p.PatientUserID == UserId).Select(n => n.Name)
                       );
            }
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationID,ReservationUserID,PatientID,Description,DateOfVisit,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Reservation reservation)
        {
            if (id != reservation.ReservationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
                    {
                        reservation.ReservationUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                    }
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

            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
                ViewData["PatientID"] = new SelectList(_context.Patients.
                        Where(p => p.PatientUserID != null).
                        Where(p => p.PatientUserID == UserId).Select(n => n.Name)
                       );
            }
            return View(reservation);
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Patients)
                .Include(r => r.ReservationAddedUser)
                .Include(r => r.ReservationUpdatedUser)
                .Include(r => r.ReservationUser)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationID == id);
        }
    }
}
