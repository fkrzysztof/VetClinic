using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.Data.Data.Clinic
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }
        public int ReservationUserID { get; set; } // klient rezerwujący
        public int? PatientID { get; set; } // pacjent

        public string Description { get; set; }
        public DateTime DateOfVisit { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    
        [ForeignKey("ReservationUserID")]
        public User ReservationUser { get; set; }

        [ForeignKey("PatientID")]
        public Patient Patients { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User ReservationAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User ReservationUpdatedUser { get; set; }
    }
}
