using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Data;

namespace VetClinic.PortalWWW.Controllers.Abstract
{
    public class BaseController : Controller
    {
        protected readonly VetClinicContext _context;
        public BaseController(VetClinicContext context)
        {
            _context = context;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("UserID") != null)
            {
                var UserFromSession = Int32.Parse(HttpContext.Session.GetString("UserID"));
                ViewBag.Patients =
                               (
                               from patients in _context.Patients
                               where patients.PatientUserID == UserFromSession
                               select patients
                               ).ToList();
                ViewBag.Doctors = _context.Users.Include(u => u.UserType).Where(w => w.UserType.Name.Contains("Lekarz") == true && w.IsActive == true);

            }


            this.OnActionExecuting(filterContext);
            var resultContext = await next();
            this.OnActionExecuted(resultContext);
        }
    }
}
