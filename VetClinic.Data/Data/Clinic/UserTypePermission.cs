using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.Clinic
{
    public class UserTypePermission
    {
        [Key]
        public int UserPermissionID { get; set; }
        public int UserTypeID { get; set; }
        public int PermissionID { get; set; }

        public bool Access { get; set; }

        //standardowe pozycje
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


        [ForeignKey("UserTypeID")]
        public UserType UserType { get; set; }

        [ForeignKey("PermissionID")]
        public Permission Permission { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User UserTypePermissionAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User UserTypePermissionUpdatedUser { get; set; }
    }
}
