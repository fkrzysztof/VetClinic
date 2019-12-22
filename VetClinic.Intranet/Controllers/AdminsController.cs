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
    }
}
