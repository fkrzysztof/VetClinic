using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Helpers;
using VetClinic.Intranet.Controllers.Abstract;
using VetClinic.Intranet.Models;
using VetClinic.Intranet.Policy;

namespace VetClinic.Intranet.Controllers
{
    public class HomeController : AbstractPolicyController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(VetClinicContext context, ILogger<HomeController> logger) : base(context)
        {
            _logger = logger;
        }

        public DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        ScheduleBlocks sb = new ScheduleBlocks();

        public async Task<IActionResult> Index()
        {
            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int usertypeid = (from item in _context.Users where item.UserID == userid select item.UserTypeID).FirstOrDefault();

            ViewBag.VetClinicContext = _context.News.Include(n => n.NewsUpdatedUser)
                                        .Include(n => n.ReceiverUserTypes).Include(n => n.SenderUser).Include(n => n.NewsReadeds)
                                        .Where(n => n.UserTypeID == usertypeid 
                                        && n.IsActive == true 
                                        && n.SenderUser.UserID != userid
                                        && n.NewsReadeds.FirstOrDefault(f => f.UserId == userid) == null)
                                        .OrderByDescending(o => o.UpdatedDate).Take(6).ToList();

            if (((List<News>)ViewBag.VetClinicContext).Any())
            {
                ViewData["Empty"] = "No Results";
            }

            ViewBag.NewMessage = _context.News
                .Include(i => i.NewsReadeds)
                .Where(w => w.UserTypeID == usertypeid && 
                    w.SenderUser.UserID != userid && 
                    w.StartDate <= DateTime.Now && 
                    w.ExpirationDate >= DateTime.Now && 
                    w.NewsReadeds.FirstOrDefault(f => f.UserId == userid) == null)
                .Count();

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
            
            if (HttpContext.Session.GetString("Login") != null)
            {
                return View(sb);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [ActionName("Index")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Navi([Bind("First,Navigation")] ScheduleBlocks c)
        {
            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            int usertypeid = (from item in _context.Users where item.UserID == userid select item.UserTypeID).FirstOrDefault();

            ViewBag.VetClinicContext = _context.News.Include(n => n.NewsUpdatedUser)
                                        .Include(n => n.ReceiverUserTypes).Include(n => n.SenderUser).Include(n => n.NewsReadeds)
                                        .Where(n => n.UserTypeID == usertypeid
                                        && n.IsActive == true
                                        && n.SenderUser.UserID != userid
                                        && n.NewsReadeds.FirstOrDefault(f => f.UserId == userid) == null)
                                        .OrderByDescending(o => o.UpdatedDate).Take(6).ToList();

            if (((List<News>)ViewBag.VetClinicContext).Any())
            {
                ViewData["Empty"] = "No Results";
            }

            ViewBag.NewMessage = _context.News
                .Include(i => i.NewsReadeds)
                .Where(w => w.UserTypeID == usertypeid &&
                    w.SenderUser.UserID != userid &&
                    w.StartDate <= DateTime.Now &&
                    w.ExpirationDate >= DateTime.Now &&
                    w.NewsReadeds.FirstOrDefault(f => f.UserId == userid) == null)
                .Count();

            if (c.Navigation == "next")
                c.First = c.First.Add(new TimeSpan(7, 0, 0, 0, 0));
            else if (c.Navigation == "previous")
                c.First -= (new TimeSpan(7, 0, 0, 0, 0));

            HttpContext.Session.SetString("day", JsonConvert.SerializeObject(c.First));

            if (HttpContext.Session.GetString("Login") != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult ToDay()
        {
            HttpContext.Session.Remove("day");
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
