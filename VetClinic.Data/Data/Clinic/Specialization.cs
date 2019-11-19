using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.Clinic
{
    public class Specialization // specjalizacje
    {
        [Key]
        public int SpecializationID { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagane")]
        [Display(Name = "Nazwa Specjalizacji:")]
        public string Name { get; set; } // nazwa specjalizacji
        [Display(Name = "Opis Specjalizacji:")]
        public string Description { get; set; }
        [Display(Name = "Czy Aktywny:")]
        public bool IsActive { get; set; }

        [Display(Name = "Data Dodania:")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Data Aktualizacji:")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Użytkownik Dodajacy:")]
        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User SpecializationAddedUser { get; set; }
        [Display(Name = "Użytkownik Modyfikujący:")]
        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User SpecializationUpdatedUser { get; set; }

        public virtual ICollection<MedicalSpecialization> MedicalSpecializations { get; set; }
    }
}
