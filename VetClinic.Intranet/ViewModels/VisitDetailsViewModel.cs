using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.Intranet.ViewModels
{
    public class VisitDetailsViewModel
    {
        public Visit Visit { get; set; }
        public Patient Patient { get; set; }
        public User User { get; set; }
        public Treatment Treatment { get; set; }
        public Medicine Medicine { get; set; }
        public List<VisitMedicine> VisitMedicine { get; set; }
    }
}
