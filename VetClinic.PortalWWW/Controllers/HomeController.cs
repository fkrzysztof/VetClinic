using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VetClinic.Data;
using VetClinic.PortalWWW.Models;

namespace VetClinic.PortalWWW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VetClinicContext _context;

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
                orderby recentnews.Position
                select recentnews
            ).ToList();

            ViewBag.ModelUser = _context.Users.Include(u => u.UserType);
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
