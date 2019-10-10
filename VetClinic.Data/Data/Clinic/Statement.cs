using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.Data.Data.Clinic
{
    public class Statement
    {
        [Key]
        public int StatementID { get; set; } // klucz główny
        public int? SenderID { get; set; } // nadawca
        public int? ReceiverID { get; set; } // odbiorca

        [Required(ErrorMessage = "Wiadomosc jest wymagana")]
        public string Message { get; set; }
        public bool IsReaded { get; set; } // czy odczytana
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
   
        [ForeignKey("SenderID")]
        public User SenderUser { get; set; }
  
        [ForeignKey("ReceiverID")]
        public User ReceiverUser { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User StatementAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User StatementUpdatedUser { get; set; }
    }
}
