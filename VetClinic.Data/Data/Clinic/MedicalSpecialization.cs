using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.Clinic
{
    public class MedicalSpecialization // specjalizacje lakarza
    {
        [Key]
        public int MedicalSpecializationID { get; set; }
        [Display(Name = "Rodzaj Użytkownika:")]
        public int? UserID { get; set; } // id lekarza==Usera
        [Display(Name = "Rodzaj Specjalizacji:")]
        public int SpecializationID { get; set; } // id specjalizacji

        [Display(Name = "Czy Aktywny:")]
        public bool IsActive { get; set; }
        [Display(Name = "Data Dodania:")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Data Aktualizacji:")]
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("UserID")]
        public User MedicalSpecializationUser { get; set; }

        [ForeignKey("SpecializationID")]
        [Display(Name = "Specjalizacja:")]
        public Specialization Specialization { get; set; }

        [Display(Name = "Użytkownik Dodajacy:")]
        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User MedicalSpecializationAddedUser { get; set; }

        [Display(Name = "Użytkownik Modyfikujący:")]
        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User MedicalSpecializationUpdatedUser { get; set; }
    }
}
