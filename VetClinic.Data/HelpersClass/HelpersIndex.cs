using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.HelpersClass
{
    public class HelpersIndex
    {
        public int UserTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Size { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Permission> permission_list { get; set; }
    }
}
