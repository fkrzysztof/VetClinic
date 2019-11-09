using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VetClinic.Data.Data.Clinic
{
    public class VisitMedicine
    {
        [Key]
        public int VisitMedicineID { get; set; }

        public int? VisitID { get; set; }
        public int? MedicineID { get; set; }

        [ForeignKey("VisitID")]
        public Visit Visit { get; set; }

        [ForeignKey("MedicineID")]
        public Medicine Medicine { get; set; }
    }
}
