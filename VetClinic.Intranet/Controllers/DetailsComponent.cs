using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Helpers;
using VetClinic.Intranet.Controllers.Abstract;

namespace VetClinic.Intranet.Controllers
{
    public class DetailsComponent : ViewComponent
    {
        private readonly VetClinicContext _context;

        public DetailsComponent(VetClinicContext context) { _context = context; }
        public async Task<IViewComponentResult> InvokeAsync(DateTime AddedDate, string AddedName, DateTime UpdatedDate, string UpdatedName)
        {
            var list = new List<string>();
            list.Add(AddedDate.ToString());
            list.Add(AddedName);
            list.Add(UpdatedDate.ToString());
            list.Add(UpdatedName);

            return View("DetailsComponent", list);
        }

    }
}
