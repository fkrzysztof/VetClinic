using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.Data.Data.Clinic
{
    public class PrescriptionItem // pozycje recepty
    {
        [Key]
        public int PrescriptionItemID { get; set; }
        public int? PrescriptionID { get; set; } // id recepty
        public int MedicineID { get; set; }

        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        [ForeignKey("PrescriptionID")]
        public Prescription Prescription { get; set; }
  
        [ForeignKey("MedicineID")]
        public Medicine Medicine { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User PrescriptionItemAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User PrescriptionItemUpdatedUser { get; set; }      
    }
}
