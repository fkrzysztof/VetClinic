using System;
using System.ComponentModel.DataAnnotations;

namespace VetClinic.Data.Data.Clinic
{
    public class ScheduleBlock
    {
        [Key]
        public int ScheduleBlockID { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan? TimeInterval { get; set; }
    }
}
