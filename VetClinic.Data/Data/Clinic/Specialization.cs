using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.Data.Data.Clinic
{
    public class Specialization // specjalizacje
    {
        [Key]
        public int SpecializationID { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagane")]
        public string Name { get; set; } // nazwa specjalizacji
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User SpecializationAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User SpecializationUpdatedUser { get; set; }

        public virtual ICollection<MedicalSpecialization> MedicalSpecializations { get; set; }
    }
}
