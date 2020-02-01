using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Helpers;
using VetClinic.PortalWWW.Controllers.Abstract;

namespace VetClinic.PortalWWW.Controllers
{
    public class ClientPanelController : BaseController
    {
        public DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        ScheduleBlocks sb = new ScheduleBlocks();

        public ClientPanelController(VetClinicContext context) : base(context) { }

        public async Task<IActionResult> Index()
        {
            ViewBag.UserFromSession = Int32.Parse(HttpContext.Session.GetString("UserID"));

            var value = HttpContext.Session.GetString("day");
            if (value != null)
            {
                DateTime day = JsonConvert.DeserializeObject<DateTime>(value);
                sb.First = day;
            }
            else
            {
                while (true)
                {
                    if (now.DayOfWeek != DayOfWeek.Monday)
                        now -= new TimeSpan(1, 0, 0, 0, 0);
                    else
                        break;
                }
                sb.First = now;
            }

            //dodaje kolekcje rezerwacji w zakresie aktualnie ogladanym
            sb.Reservation = _context.Reservations.Where(w => w.DateOfVisit >= now && w.DateOfVisit <= now.AddDays(7) && w.IsActive == true).ToList();
            //dodanie kolekcji dni wolnych
            sb.InaccessibleDay = _context.InaccessibleDays.Select(s => s.Date).ToList();
            //dodanie kolekcji godzin pracy przychodni
            sb.ScheduleBlock = _context.ScheduleBlocks.OrderBy(o => o.Time).ToList();

            var UserFromSession = Int32.Parse(HttpContext.Session.GetString("UserID"));
            ViewBag.Patients =
                           (
                           from patients in _context.Patients
                           where patients.PatientUserID == UserFromSession
                           select patients
                           ).ToList();
            return View(sb);
        }

        [ActionName("Index")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Navi([Bind("First,Navigation")] ScheduleBlocks c)
        {
            ViewBag.UserFromSession = Int32.Parse(HttpContext.Session.GetString("UserID"));

            if (c.Navigation == "next")
                c.First = c.First.Add(new TimeSpan(7, 0, 0, 0, 0));
            else if (c.Navigation == "previous")
                c.First -= (new TimeSpan(7, 0, 0, 0, 0));

            HttpContext.Session.SetString("day", JsonConvert.SerializeObject(c.First));
            ViewBag.Doctors = _context.Users.Include(u => u.UserType).Where(w => w.UserType.Name.Contains("Lekarz") == true && w.IsActive == true);
            
            return RedirectToAction("Index");
        }
        public IActionResult ToDay()
        {
            HttpContext.Session.Remove("day");
            return RedirectToAction("Index");
        }
    }
}