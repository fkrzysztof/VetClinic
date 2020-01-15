using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Data;
using VetClinic.Intranet.Controllers.Abastract;

namespace VetClinic.Intranet.Controllers
{
    public class DoctorsController : AbstractUsersController
    {
        public DoctorsController(VetClinicContext context)
            : base(context, 2)
        {
        }

        public override async Task<IActionResult> Index(string searchString)
        {
            ViewBag.Title = "Lekarze";
            ViewBag.New = "Dodaj";

            return await base.Index(searchString);
        }

        public override async Task<IActionResult> Create()
        {
            ViewBag.Title = "Nowy lekarz";
            ViewBag.Button = "Dodaj";

            return await base.Create();
        }

        public override async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Title = "Edycja";
            ViewBag.Delete = "Usuń";
            ViewBag.Restore = "Przywróć";
            ViewBag.Controller = "Doctors";

            return await base.Edit(id);
        }
    }
}
