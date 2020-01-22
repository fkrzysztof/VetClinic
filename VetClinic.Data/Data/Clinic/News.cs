using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.Clinic
{
    public class News
    {
        [Key]
        public int NewsID { get; set; } // klucz główny
        public int? UserID { get; set; } // nadawca
        [Required(ErrorMessage = "Grupa odbiorców jest wymagana")]
        public int UserTypeID { get; set; } // grupa odbiorców
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [StringLength(40, ErrorMessage = "Maksymalnie 40 znaków")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Wiadomosc jest wymagana")]
        public string Message { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Data początkowa jest wymagana")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Data końcowa jest wymagana")]
        public DateTime ExpirationDate { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
   
        [ForeignKey("UserID")]
        public User SenderUser { get; set; }
  
        [ForeignKey("UserTypeID")]
        public UserType ReceiverUserTypes { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User NewsUpdatedUser { get; set; }

        public ICollection<NewsReaded> NewsReadeds { get; set; }
    }
}
