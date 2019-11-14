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
        public async Task<IActionResult> Index()
        {
            ViewBag.ModelUser =
            (
                from user in _context.Users
                orderby user.UserID 
                select User
            ).ToList();
            var vetClinicContext = _context.Users.Include(u => u.UserType);
            return View(await vetClinicContext.ToListAsync());
        }

    }
}
