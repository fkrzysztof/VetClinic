using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data;
using VetClinic.Intranet.Controllers.Abastract;

namespace VetClinic.Intranet.Controllers
{
    public class AdminsController : AbstractUsersController
    {
        public AdminsController(VetClinicContext context)
            : base(context, 1)
        {
        }

        public override async Task<IActionResult> Index(string searchString)
        {
            ViewBag.Title = "Administratorzy";
            ViewBag.New = "Dodaj";

            return await base.Index(searchString);
        }

        public override async Task<IActionResult> Create()
        {
            ViewBag.Title = "Nowy administrator";
            ViewBag.Button = "Dodaj";

            return await base.Create();
        }

        public override async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Title = "Edycja";
            ViewBag.Delete = "Usuń";
            ViewBag.Restore = "Przywróć";
            ViewBag.Controller = "Admins";

            return await base.Edit(id);
        }
    }
}
