using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data;

namespace VetClinic.PortalWWW.Controllers
{
    public class RecentNewsController : Controller
    {
        private readonly VetClinicContext _context;

        public RecentNewsController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: RecentNews
        public  IActionResult Index(int id)
        {
            ViewBag.ModelUser =
               (
               from crew in _context.Users
               orderby crew.AddedDate
               select crew
               ).ToList();

            ViewBag.ModelRecentNews =
             (
                 from recentnews in _context.RecentNews
                 orderby recentnews.Position
                 where recentnews.IsActive==true
                 select recentnews
             ).ToList();
            var item = _context.RecentNews.Find(id);
            return View(item);
        }

    }
}