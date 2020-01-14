using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data;
using VetClinic.Data.Helpers;

namespace VetClinic.PortalWWW.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly VetClinicContext _context;

        SmtpConfiguration SmtpConf = new SmtpConfiguration();

        public ResetPasswordController(VetClinicContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string Password, string NewPassword, string NewPasswordConfirm)
        {
            return View();
        }
    }
}