using VetClinic.Data;

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
