using System.Collections.Generic;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.HelpersClass
{
    public class UserTypesDetails
    {
        public UserType typeUser { get; set; }
        public ICollection<Permission> permissionsUser { get; set; }
        public ICollection<User>  users { get; set; }  
        public ICollection<Permission> permissionsNotSelected { get; set; }  
    }
}
