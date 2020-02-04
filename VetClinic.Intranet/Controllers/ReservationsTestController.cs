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
    public class ReservationsTestController : Controller
    {
        private readonly VetClinicContext _context;

        public ReservationsTestController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: ReservationsTest
        public async Task<IActionResult> Index()
        {
            var vetClinicContext = _context.Reservations.Include(r => r.Patients).Include(r => r.ReservationAddedUser).Include(r => r.ReservationUpdatedUser).Include(r => r.ReservationUser).Include(r => r.Visit);
            return View(await vetClinicContext.ToListAsync());
        }

        // GET: ReservationsTest/Details/5
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
                .Include(r => r.Visit)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: ReservationsTest/Create
        public IActionResult Create()
        {
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name");
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["ReservationUserID"] = new SelectList(_context.Users, "UserID", "City");
            ViewData["VisitID"] = new SelectList(_context.Visits, "VisitID", "VisitID");
            return View();
        }

        // POST: ReservationsTest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationID,ReservationUserID,PatientID,Description,DateOfVisit,IsActive,AddedDate,UpdatedDate,VisitID,AddedUserID,UpdatedUserID")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name", reservation.PatientID);
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", reservation.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", reservation.UpdatedUserID);
            ViewData["ReservationUserID"] = new SelectList(_context.Users, "UserID", "City", reservation.ReservationUserID);
            ViewData["VisitID"] = new SelectList(_context.Visits, "VisitID", "VisitID", reservation.VisitID);
            return View(reservation);
        }

        // GET: ReservationsTest/Edit/5
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
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name", reservation.PatientID);
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", reservation.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", reservation.UpdatedUserID);
            ViewData["ReservationUserID"] = new SelectList(_context.Users, "UserID", "City", reservation.ReservationUserID);
            ViewData["VisitID"] = new SelectList(_context.Visits, "VisitID", "VisitID", reservation.VisitID);
            return View(reservation);
        }

        // POST: ReservationsTest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationID,ReservationUserID,PatientID,Description,DateOfVisit,IsActive,AddedDate,UpdatedDate,VisitID,AddedUserID,UpdatedUserID")] Reservation reservation)
        {
            if (id != reservation.ReservationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "City", reservation.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "City", reservation.UpdatedUserID);
            ViewData["ReservationUserID"] = new SelectList(_context.Users, "UserID", "City", reservation.ReservationUserID);
            ViewData["VisitID"] = new SelectList(_context.Visits, "VisitID", "VisitID", reservation.VisitID);
            return View(reservation);
        }

        // GET: ReservationsTest/Delete/5
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
                .Include(r => r.Visit)
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: ReservationsTest/Delete/5
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
