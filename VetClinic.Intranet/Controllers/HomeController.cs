using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VetClinic.Data;
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

        public async Task<IActionResult> Index()
        {
            //UserPolicy policy = new UserPolicy(_context, HttpContext, this.ControllerContext.RouteData);
            //ViewData = policy.PopulateViewData(ViewData);

            if (HttpContext.Session.GetString("Login") != null)
            {
                return View();
            }else
            return RedirectToAction("Index", "Login");
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
