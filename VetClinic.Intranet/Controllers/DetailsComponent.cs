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

namespace VetClinic.Intranet.Controllers
{
    public class DetailsComponent : ViewComponent
    {
        private readonly VetClinicContext _context;

        public DetailsComponent(VetClinicContext context) { _context = context; }
        public async Task<IViewComponentResult> InvokeAsync(DateTime AddedDate, String AddedName, DateTime UpdatedDate, String UpdatedName)
        {
            var list = new List<string>();

            if (AddedDate == null) list.Add("Brak danych");
                else list.Add(AddedDate.ToString());
            if (AddedName == null) list.Add("Brak danych"); 
                else list.Add(AddedName);
            if(UpdatedDate == null) list.Add("Brak danych"); 
                else list.Add(UpdatedDate.ToString());
            if (UpdatedName == null) list.Add("Brak danych");
                else list.Add(UpdatedName);

            return await Task.Run(() => View("DetailsComponent", list));
            //return View("DetailsComponent", list);

        }

    }
}
