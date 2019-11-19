using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.Clinic
{
    public class Treatment // zabiegi
    {
        [Key]
        public int TreatmentID { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagane")]
        public string Name { get; set; } // nazwa zabiegu
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; } 

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User TreatmentAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User TreatmentUpdatedUser { get; set; }

        public ICollection<Visit> Visits { get; set; }
    }
}
