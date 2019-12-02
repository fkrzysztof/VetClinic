using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VetClinic.Data;
using VetClinic.Data.Helpers;

namespace VetClinic.Intranet.Controllers
{
    public class CalendarController : Controller
    {
        public DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        ScheduleBlocks sb = new ScheduleBlocks();
        private readonly VetClinicContext _context;

        public CalendarController(VetClinicContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            while (true)
            {
                if (now.DayOfWeek != DayOfWeek.Monday)
                    now -= new TimeSpan(1, 0, 0, 0, 0);
                else
                    break;
            }
            sb.First = now;
            //dodaje kolekcje rezerwacji w zakresie aktualnie ogladanym
            sb.Reservation = _context.Reservations.Where(w => w.DateOfVisit >= now && w.DateOfVisit <= now.AddDays(7)).ToList();
            //sb.Reservation = _context.Reservations.ToList();

            //dodanie kolekcji dni wolnych
            sb.InaccessibleDay = _context.InaccessibleDays.Select(s => s.Date).ToList();
            //dodanie kolekcji godzin pracy przychodni
            sb.ScheduleBlock = _context.ScheduleBlocks.OrderBy(o => o.Time).ToList();
            ViewBag.UserID = null;

            return View(sb);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Index([Bind("First,Navigation")] ScheduleBlocks c)
        {
            if (c.Navigation == "next")
                //now = now.Add(new TimeSpan(7, 0, 0, 0, 0));
                c.First = c.First.Add(new TimeSpan(7, 0, 0, 0, 0));
            if (c.Navigation == "previous")
                c.First -= (new TimeSpan(7, 0, 0, 0, 0));
            sb.First = c.First;
            sb.Reservation = _context.Reservations.Where(w => w.DateOfVisit >= sb.First && w.DateOfVisit <= sb.First.AddDays(7)).ToList();
            //dodanie kolekcji dni wolnych
            sb.InaccessibleDay = _context.InaccessibleDays.Select(s => s.Date).ToList();
            //dodanie kolekcji godzin pracy przychodni
            sb.ScheduleBlock = _context.ScheduleBlocks.OrderBy(o => o.Time).ToList();

            return View(sb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateFromCalendar(DateTime DateOfVisit)
        {

            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name");
            ViewData["ReservationUserID"] = new SelectList(_context.Users, "UserID", "LastName");
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
                reservation.DateOfVisit = DateOfVisitFromCalendar;
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientID", "Name", reservation.PatientID);
            ViewData["AddedUserID"] = new SelectList(_context.Users, "UserID", "LastName", reservation.AddedUserID);
            ViewData["UpdatedUserID"] = new SelectList(_context.Users, "UserID", "LastName", reservation.UpdatedUserID);
            ViewData["ReservationUserID"] = new SelectList(_context.Users, "UserID", "LastName", reservation.ReservationUserID);
            return View(reservation);
        }

    }
}

