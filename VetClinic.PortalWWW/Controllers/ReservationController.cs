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




namespace VetClinic.PortalWWW.Controllers
{
    public class ReservationController : Controller
    {
        private readonly VetClinicContext _context;

        public ReservationController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            ViewBag.Doctors = _context.Users.Include(u => u.UserType).Where(w => w.UserType.Name.Contains("Lekarz") == true && w.IsActive == true);
            int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
            var vetClinicContext = _context.Reservations.Where(r=>r.ReservationUserID==UserId).Include(r => r.Patients).Include(r => r.ReservationAddedUser).Include(r => r.ReservationUpdatedUser).Include(r => r.ReservationUser);
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Doctors = _context.Users.Include(u => u.UserType).Where(w => w.UserType.Name.Contains("Lekarz") == true && w.IsActive == true);
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

        //Czesc Bartek
        //do poprawienia masz swoja czesc zadania, ja robie tylko date wizyty z kalendarza!!
        //Krzysztof Franczyk
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(DateTime DateOfVisit)
        {
            ViewBag.Doctors = _context.Users.Include(u => u.UserType).Where(w => w.UserType.Name.Contains("Lekarz") == true && w.IsActive == true);
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
        public IActionResult Create()
        {

            ViewBag.Doctors = _context.Users.Include(u => u.UserType).Where(w => w.UserType.Name.Contains("Lekarz") == true && w.IsActive == true);
            int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
            ViewData["PatientID"] = new SelectList(_context.Patients.Where(p=>p.PatientUserID == UserId), "PatientID", "Name");
            //ViewData["BlokCzasowy"] = new SelectList(_context.ScheduleBlocks.OrderBy(b => b.Time), "ScheduleBlockID", "Time");
            return View();
        }



       

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Doctors = _context.Users.Include(u => u.UserType).Where(w => w.UserType.Name.Contains("Lekarz") == true && w.IsActive == true);
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
            ViewBag.Doctors = _context.Users.Include(u => u.UserType).Where(w => w.UserType.Name.Contains("Lekarz") == true && w.IsActive == true);
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
            ViewBag.Doctors = _context.Users.Include(u => u.UserType).Where(w => w.UserType.Name.Contains("Lekarz") == true && w.IsActive == true);
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
