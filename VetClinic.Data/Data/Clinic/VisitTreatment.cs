using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VetClinic.Data.Data.Clinic
{
    public class VisitTreatment
    {
        [Key]
        public int VisitTreatmentID { get; set; }

        public int? VisitID { get; set; }
        public int? TreatmentID { get; set; }

        [ForeignKey("VisitID")]
        public Visit Visit { get; set; }

        [ForeignKey("TreatmentID")]
        public Treatment Treatment { get; set; }
    }
}
