using VetClinic.Data;
using VetClinic.Intranet.Controllers.Abastract;

namespace VetClinic.Intranet.Controllers
{
    public class EmployeesController : AbstractUsersController
    {
        public EmployeesController(VetClinicContext context)
            : base(context, 3)
        {
        }
    }
}
