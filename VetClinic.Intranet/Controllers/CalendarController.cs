using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            sb.Reservation = _context.Reservations.Where(w => w.DateOfVisit >= now && w.DateOfVisit <= now.AddDays(7) && w.IsActive == true).ToList();
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
                c.First = c.First.Add(new TimeSpan(7, 0, 0, 0, 0));
            if (c.Navigation == "previous")
                c.First -= (new TimeSpan(7, 0, 0, 0, 0));
            sb.First = c.First;
            sb.Reservation = _context.Reservations.Where(w => w.DateOfVisit >= sb.First && w.DateOfVisit <= sb.First.AddDays(7) && w.IsActive == true).ToList();
            //dodanie kolekcji dni wolnych
            sb.InaccessibleDay = _context.InaccessibleDays.Select(s => s.Date).ToList();
            //dodanie kolekcji godzin pracy przychodni
            sb.ScheduleBlock =  _context.ScheduleBlocks.OrderBy(o => o.Time).ToList();

            return View(sb);
        }
    }
}

