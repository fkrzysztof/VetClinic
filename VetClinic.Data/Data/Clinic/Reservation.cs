using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.Clinic
{
    public class Reservation
    {
        [Key]
        [Display(Name = "Id rezerwacji")]
        public int ReservationID { get; set; }
        [Display(Name = "Klient")]
        [Required(ErrorMessage = "Uzupełnij pole")]
        public int ReservationUserID { get; set; } // klient rezerwujący

        [Display(Name = "Pacjent")]
        [Required(ErrorMessage ="Uzupełnij pole")]
        public int? PatientID { get; set; } // pacjent

        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Data wizyty")]
        public DateTime DateOfVisit { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Data dodania")]
        public DateTime AddedDate { get; set; }
        [Display(Name = "Aktualizacja")]
        public DateTime? UpdatedDate { get; set; }

        public int? VisitID { get; set; }
        [ForeignKey("VisitID")]
        [Display(Name = "Wizyta")]
        public Visit Visit { get; set; }

        [Display(Name = "Zarezerwował")]
        [ForeignKey("ReservationUserID")]
        public User ReservationUser { get; set; }
        [Display(Name = "Pacjent")]
        [ForeignKey("PatientID")]
        public Patient Patients { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        [Display(Name = "Zarezerwował")]
        public User ReservationAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        [Display(Name = "Zaktualizował")]
        public User ReservationUpdatedUser { get; set; }
        
    }
}
