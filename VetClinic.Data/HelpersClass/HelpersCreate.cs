﻿using System;
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
    
        
      // public ICollection<UserTypePermission> permissions_list { get; set; }
       public ICollection<Permission> permissions_list { get; set; }








    }
}
