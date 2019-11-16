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
            var vetClinicContext = _context.Reservations.Include(r => r.Patients).Include(r => r.ReservationAddedUser).Include(r => r.ReservationUpdatedUser).Include(r => r.ReservationUser);
            return View(await vetClinicContext.ToListAsync());
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

        // GET: Reservation/Create
        public IActionResult Create()
        {
            if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
            {
                int UserId = Int32.Parse(HttpContext.Session.GetString("UserID"));
                ViewData["PatientID"] = new SelectList(_context.Patients.
                        Where(p => p.PatientUserID != null).
                        Where(p => p.PatientUserID == UserId).Select(n => n.Name)
                       );
            }

            //ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            return View();
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationID,ReservationUserID,PatientID,Description,DateOfVisit,IsActive,AddedDate,UpdatedDate,AddedUserID,UpdatedUserID")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
                {
                    reservation.AddedUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                    reservation.ReservationUserID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                }
                reservation.AddedDate = DateTime.Now;
                reservation.IsActive = true;
                _context.Add(reservation);
                await _context.SaveChangesAsync();
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
