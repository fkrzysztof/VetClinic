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
        public async Task<IViewComponentResult> InvokeAsync(DateTime? AddedDate, int? AddedUserID, DateTime? UpdatedDate, int? UpdatedUserID)
        {
            var list = new Dictionary<string, string>();

            if (AddedDate != null) list.Add("Data dodania:", AddedDate.ToString());
                else list.Add("Data dodania:", "");

            if (UpdatedDate != null) list.Add("Data modyfikacji:", UpdatedDate.ToString());
                else list.Add("Data modyfikacji:", "");

            if (AddedUserID != null)
            {
                var user = await _context.Users.FindAsync(AddedUserID);
                if (user != null) list.Add("Dodał:", $"{user.FirstName} {user.LastName}");
            }
            else list.Add("Dodał:", "");


            if (UpdatedUserID != null)
            {
                var user = await _context.Users.FindAsync(UpdatedUserID);
                if (user != null) list.Add("Modyfikował:", $"{user.FirstName} {user.LastName}");
            }
            else list.Add("Modyfikował:", "");


            return await Task.Run(() => View("DetailsComponent", list));
        }

    }
}
