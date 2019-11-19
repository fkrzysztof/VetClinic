using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.Clinic
{
    public class PatientType // rodzaj pacjenta: kot, pies ...
    {
        [Key]
        public int PatientTypeID { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagane")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Czy aktywny")]
        public bool IsActive { get; set; }
        [Display(Name = "Data dodania")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Data modyfikacji")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Użytkownik dodający")]
        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]

        [Display(Name = "Użytkownik dodający rodzaj pacjenta")]
        public User PatientTypeAddedUser { get; set; }

        [Display(Name = "Użytkownik modyfikujący")]
        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        [Display(Name = "Użytkownik modyfikujący rodzaj pacjenta")]
        public User PatientTypeUpdatedUser { get; set; }

        public ICollection<Patient> Patients { get; set; }
    }
}
