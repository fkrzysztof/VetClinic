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
    }
}
