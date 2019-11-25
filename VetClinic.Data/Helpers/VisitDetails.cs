using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Helpers
{
    public class VisitDetails
    {
        public Visit Visit { get; set; }
        public Patient Patient { get; set; }
        public User User { get; set; }
        public Treatment Treatment { get; set; }
        public Medicine Medicine { get; set; }
        public MedicineType MedicineType { get; set; }
        public List<VisitMedicine> VisitMedicine { get; set; }
        public List<VisitTreatment> VisitTreatments { get; set; }
    }
}
