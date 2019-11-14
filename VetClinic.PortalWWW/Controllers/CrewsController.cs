using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.PortalWWW.Controllers
{
    public class CrewsController : Controller
    {
        private readonly VetClinicContext _context;

        public CrewsController(VetClinicContext context)
        {
            _context = context;
        }

        // GET: Crews
        public async Task<IActionResult> Index(int? id)
        {
            if(id == null)
                return View("Views/Home/Index.cshtml");


            ViewBag.ModelRecentNews =
            (
                from recentnews in _context.RecentNews
                orderby recentnews.Position
                select recentnews
            ).ToList();

            ViewBag.ModelUser = _context.Users.Include(u => u.UserType);


            User result  = _context.Users.FirstOrDefault(i => i.UserID == id);
            return View(result);
        }

    }
}
