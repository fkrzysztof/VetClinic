using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.Data.Data.Clinic
{
    public class Operation // Zabiegi
    {
        [Key]
        public int OperationID { get; set; }
        public int? VisitID { get; set; } // wizyta
        public int? VetID { get; set; } // weterynarz

        public DateTime DateOfOperation { get; set; } // data zabiegu
        public string Description { get; set; } // krótki opis co się dzieje podczas zgłaszania wizyty
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
     
        [ForeignKey("VisitID")]
        public Visit Visit { get; set; }
        
        [ForeignKey("VetID")]
        public User VetUser { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User OperationAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User OperationUpdatedUser { get; set; }
    }
}
