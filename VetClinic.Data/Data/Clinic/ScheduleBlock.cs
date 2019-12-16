using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace VetClinic.Data.Data.Clinic
{
    public class ScheduleBlock
    {
        [Key]
        public int ScheduleBlockID { get; set; }
        [DataType(DataType.Time, ErrorMessage ="Wprowadź poprawnt czas np 8:30")]
        [Display(Name = "Czas")]
        [Required(ErrorMessage ="czas jest wymagany")]
        public TimeSpan Time { get; set; }
        [DataType(DataType.Time, ErrorMessage = "Wprowadź poprawnt czas np 8:30")]
        [Display(Name = "Przedział czasowy")]
        public TimeSpan? TimeInterval { get; set; }
    }
}
