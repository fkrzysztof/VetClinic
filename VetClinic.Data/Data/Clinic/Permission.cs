using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VetClinic.Data.Data.Clinic
{
    public class Permission
    {
        [Key]
        public int PermissionID { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagane")]
        public string Name { get; set; } // nazwa uprawwnienia
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User PermissionAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User PermissionUpdatedUser { get; set; }

    }
}
