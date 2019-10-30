using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.Data.Data.Clinic
{
    public class MedicalSpecialization // specjalizacje lakarza
    {
        [Key]
        public int MedicalSpecializationID { get; set; }
        public int? UserID { get; set; } // id lekarza==Usera
        public int SpecializationID { get; set; } // id specjalizacji

        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("UserID")]
        public User MedicalSpecializationUser { get; set; }

        [ForeignKey("SpecializationID")]
        public Specialization Specialization { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User MedicalSpecializationAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User MedicalSpecializationUpdatedUser { get; set; }
    }
}
