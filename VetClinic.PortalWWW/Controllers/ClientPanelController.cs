﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data;

namespace VetClinic.PortalWWW.Controllers
{
    public class ClientPanelController : Controller
    {
        private readonly VetClinicContext _context;

        public ClientPanelController(VetClinicContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}