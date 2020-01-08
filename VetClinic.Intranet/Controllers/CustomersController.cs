﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data;
using VetClinic.Intranet.Controllers.Abastract;

namespace VetClinic.Intranet.Controllers
{
    public class CustomersController : AbstractUsersController
    {
        public CustomersController(VetClinicContext context)
            : base(context, 4)
        {
        }

        public override async Task<IActionResult> Index(string searchString)
        {
            ViewBag.Title = "Klienci";
            ViewBag.New = "Dodaj nowego klienta";

            return await base.Index(searchString);
        }

        public override async Task<IActionResult> Create()
        {
            ViewBag.Title = "Nowy klient";
            ViewBag.Button = "Dodaj klienta";

            return await base.Create();
        }

        public override async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Title = "Edycja klienta";
            ViewBag.Delete = "Usuń klienta";
            ViewBag.Restore = "Przywróć klienta";
            ViewBag.Controller = "Customers";

            return await base.Edit(id);
        }
    }
}
