using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.HelpersClass
{
    public class HelpersCreate
    {
        // public ICollection<UserType> userTypes_list {get; set;}
        public int UserTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    
        
        //UserTypeID = 0,
        //        Name = "wpisz",
        //        Description = "wpisz wiecej",
        //        IsActive = true,
            //    permissions_list = permissions_list_add


       //2
       public ICollection<UserTypePermission> permissions_list { get; set; }







    }
}
