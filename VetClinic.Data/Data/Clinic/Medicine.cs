using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.Clinic
{
    public class Medicine // leki
    {
        [Key]
        public int MedicineID { get; set; }
        public int MedicineTypeID { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [Display(Name ="Nazwa")]
        public string Name { get; set; }

        [Display(Name="Opis")]
        public string Description { get; set; }

        [Display(Name ="Cena")]
        [Required(ErrorMessage = "Cena jest wymagana")]
        //[DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }          
        
        [ForeignKey("MedicineTypeID")]
        public MedicineType MedicineType { get; set; } 

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User MedicineAddedUser { get; set; }
              
        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User MedicineUpdatedUser { get; set; }

        public virtual ICollection<VisitMedicine> VisitMedicines { get; set; }
    }
}
