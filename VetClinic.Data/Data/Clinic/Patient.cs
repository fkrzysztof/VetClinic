using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.Clinic
{
    public class Patient // pacjenci
    {
        [Key]
        public int PatientID { get; set; }
        [Display(Name = "Rodzaj zwierzaka")]
        public int PatientTypeID { get; set; } // rodzaj zwierzaka
        [Display(Name = "Właściciel")]
        public int? PatientUserID { get; set; } // właściciel zwierzaka

        [Required(ErrorMessage = "Imie jest wymagane")]
        [Display(Name = "Imie")]
        public string Name { get; set; }
        [Display(Name = "Data urodzenia")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Numer pacjenta")]
        public string PatientNumber { get; set; }
        [Display(Name = "Czy aktywny")]
        public bool IsActive { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Przydomek hodowlany")]
        public string KennelName { get; set; }
        [Display(Name = "Data dodania")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Data modyfikacji")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Typ pacjenta")]
        [ForeignKey("PatientTypeID")]
        public PatientType PatientType { get; set; }

        [Display(Name = "Właściciel")]
        [ForeignKey("PatientUserID")]
        public User PatientUser { get; set; }

        [Display(Name = "Użytkownik dodający")]
        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        [Display(Name = "Dodający")]
        public User PatientAddedUser { get; set; }

        [Display(Name = "Użytkownik modyfikujący")]
        public int? UpdatedUserID { get; set; }
        [ForeignKey("UpdatedUserID")]
        [Display(Name = "Modyfikujący")]
        public User PatientUpdatedUser { get; set; }

        public ICollection<Visit> Visits { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
