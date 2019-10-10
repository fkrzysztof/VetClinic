using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.Data.Data.Clinic
{
    public class Patient // pacjenci
    {
        [Key]
        public int PatientID { get; set; }
        public int PatientTypeID { get; set; } // rodzaj zwierzaka
        public int? PatientUserID { get; set; } // właściciel zwierzaka

        [Required(ErrorMessage = "Imie jest wymagane")]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string PatientNumber { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
   
        [ForeignKey("PatientTypeID")]
        public PatientType PatientType { get; set; }    
   
        [ForeignKey("PatientUserID")]
        public User PatientUser { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User PatientAddedUser { get; set; }

        public int? UpdatedUserID { get; set; }
        [ForeignKey("UpdatedUserID")]
        public User PatientUpdatedUser { get; set; }

        public ICollection<Visit> Visits { get; set; }
    }
}
