using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.Clinic
{
    public class UserType
    {
        [Key]
        public int UserTypeID { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagane")]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [InverseProperty("ReceiverUserTypes")]
        public ICollection<News> ReceiversNews { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User UserTypeAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User UserTypeUpdatedUser { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<UserTypePermission> UserTypePermissions { get; set; }

        
    }
}
