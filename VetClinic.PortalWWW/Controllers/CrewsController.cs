using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.PortalWWW.Controllers.Abstract;

namespace VetClinic.PortalWWW.Controllers
{
    public class CrewsController : BaseController
    {
        private readonly VetClinicContext _context;

        public CrewsController(VetClinicContext context) : base(context) { }

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

            User result  = _context.Users.FirstOrDefault(i => i.UserID == id);
            ViewBag.Logged = !String.IsNullOrEmpty(HttpContext.Session.GetString("UserID"));
            
            return View(result);
        }


    }
}
