using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data.Helpers;

namespace VetClinic.Intranet.Controllers
{
    public class CalendarController : Controller
    {
        public DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        CalendarTime ct = new CalendarTime();
        
        
        public async Task<IActionResult> Index()
        {
            while (true)
            {
                if (now.DayOfWeek != DayOfWeek.Monday)
                   now -= new TimeSpan(1, 0, 0, 0, 0);
                else
                    break;
            }
            ct.First = now;

        //return View( new CalendarTime { First = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0)});
        return View(ct);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        //public IActionResult Index (string navigation)
        public async Task<IActionResult> Index ( [Bind("First,Navigation")] CalendarTime c)
        {
            if (c.Navigation == "next")
                //now = now.Add(new TimeSpan(7, 0, 0, 0, 0));
                c.First = c.First.Add(new TimeSpan(7, 0, 0, 0, 0));
            if (c.Navigation == "previous")
                c.First -= (new TimeSpan(7, 0, 0, 0, 0));
            ct.First = c.First;
            return View(ct);
        }
    }
}