using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.Clinic
{
    public class MedicineType // rodzaje leków
    {
        [Key]
        public int MedicineTypeID { get; set; }

        [Required(ErrorMessage = "Typ leku jest wymagany")]
        [Display(Name ="Typ leku")]
        public string Name { get; set; }

        [Display(Name ="Opis")]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User MedicineTypeAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User MedicineTypeUpdatedUser { get; set; }

        public ICollection<Medicine> Medicines { get; set; }
    }
}
