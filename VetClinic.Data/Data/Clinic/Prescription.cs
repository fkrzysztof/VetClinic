using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.Data.Data.Clinic
{
    public class Prescription // Recepty
    {
        [Key]
        public int PrescriptionID { get; set; }
        public int VisitID { get; set; }

        [Required(ErrorMessage = "Opis jest wymagany")]
        public string Description { get; set; }
        public decimal TotalToPay { get; set; } // do zapłaty brutto
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
     
        [ForeignKey("VisitID")]
        public virtual Visit Visit { get; set; }

        [ForeignKey("PrescriptionAddedUser")]
        public int? AddedUserID { get; set; } // użytkownik dodający
        public User PrescriptionAddedUser { get; set; }

        [ForeignKey("PrescriptionUpdatedUser")]
        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        public User PrescriptionUpdatedUser { get; set; }

        [InverseProperty("Prescription")]
        public virtual ICollection<PrescriptionItem> PrescriptionItems { get; set; }
    }
}
