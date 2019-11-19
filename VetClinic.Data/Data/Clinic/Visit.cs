using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.Clinic
{
    public class Visit // wizyty
    {
        [Key]
        public int VisitID { get; set; }
        public int VisitUserID { get; set; } // klient 
        public int? PatientID { get; set; } // zwierzak
        public int? VetID { get; set; } // weterynarz
        public int TreatmentID { get; set; } // zabieg

        public DateTime DateOfVisit { get; set; }
        public string Description { get; set; } // krótki opis co się dzieje podczas zgłaszania wizyty
        public bool IsActive { get; set; } 
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("TreatmentID")]
        public Treatment Treatment { get; set; }

        [ForeignKey("VisitUserID")]
        public User VisitUser { get; set; }
    
        [ForeignKey("PatientID")]
        public Patient Patient { get; set; }

        [ForeignKey("VetID")]
        public User VetUser { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User VisitAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User VisitUpdatedUser { get; set; }

        public virtual ICollection<VisitMedicine> VisitMedicines { get; set; }
    }
}
