using VetClinic.Data;

namespace VetClinic.Intranet.Controllers
{
    public class DoctorsController : AbstractUsersController
    {
        public DoctorsController(VetClinicContext context)
            : base(context, 2)
        {
        }
    }
}
