using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Helpers
{
    public class ScheduleBlocks
    {
        public DateTime First { get; set; }
        public string Navigation { get; set; }
        public ICollection<Reservation> Reservation { get; set; }
        public ICollection<ScheduleBlock> ScheduleBlock { get; set; }
        public ICollection<DateTime> InaccessibleDay { get; set; }
    }
}
