using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VetClinic.Data.Data.Clinic
{
    public class ScheduleBlock
    {
        [Key]
        public int ScheduleBlockID { get; set; }
        public TimeSpan Time { get; set; }
    }
}
