using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VetClinic.Data;
using VetClinic.PortalWWW.Controllers.Abstract;
using VetClinic.PortalWWW.Models;

namespace VetClinic.PortalWWW.Controllers
{
    public class HomeController : Controller
    {
        private readonly VetClinicContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, VetClinicContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.ModelRecentNews =
            (
                from recentnews in _context.RecentNews
                orderby recentnews.Position, recentnews.AddedDate descending
                where recentnews.IsActive == true
                select recentnews
            ).ToList();

            ViewBag.Doctors = _context.Users.Include(u => u.UserType).Where(w => w.UserType.Name.Contains("Lekarz") == true && w.IsActive == true);


            return View();
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
