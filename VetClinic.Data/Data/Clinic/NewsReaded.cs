using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VetClinic.Data.Data.Clinic
{
    public class NewsReaded
    {
        public int NewsReadedID { get; set; }

        public int NewsID { get; set; }
        [ForeignKey("NewsID")]
        public News News { get; set; }

        public int UserId { get; set; }


    }
}
