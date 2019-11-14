using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.Data.HelpersClass
{
    public class HelpersDetails
    {
        public UserType typeUser { get; set; }
        public ICollection<UserTypePermission> permissionsUser { get; set; }
        public ICollection<User>  users { get; set; }  
        public ICollection<Permission> permissionsNotSelected { get; set; }  

        
    }
}
