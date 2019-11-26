using System;
using System.ComponentModel.DataAnnotations;

namespace VetClinic.Data.Data.Clinic
{
    public class InaccessibleDay
    {
        [Key]
        public int InaccessibleDayID { get; set; }
        public DateTime Date { get; set; }
    }
}
