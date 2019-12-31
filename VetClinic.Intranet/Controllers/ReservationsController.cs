using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return View(await vetClinicContext.OrderByDescending(u => u.UpdatedDate).ToListAsync());
        }

        //Czesc Bartek
        //do poprawienia masz swoja czesc zadania, ja robie tylko date wizyty z kalendarza!!
        //Krzysztof Franczyk
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNew(DateTime DateOfVisit)
        {

            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name");
            ViewData["ReservationUserID"] = new SelectList(from user in _context.Users where user.IsActive == true where user.UserTypeID == 4 select new { user.UserID, Display_Name = user.FirstName + " " + user.LastName }, "UserID", "Display_Name");
            ViewData["DateOfVisit"] = DateOfVisit;

            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime DateOfVisitFromCalendar, [Bind("ReservationID,ReservationUserID,PatientID,Description")] Data.Data.Clinic.Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                reservation.AddedDate = DateTime.Now;
                reservation.IsActive = true;
                reservation.AddedUserID = 2; //to ma byc z logowania podabnie jak updateuser w edit
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
 

            ViewData["PatientID"] = new SelectList(_context.Patients.
            Where(p => p.PatientUserID != null).
            Where(p => p.PatientUserID == reservation.ReservationUserID).Select(n => n.Name)
           );

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
            if (id != reservation.ReservationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
    }
}
