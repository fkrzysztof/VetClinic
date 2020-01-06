using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VetClinic.Data.Data.Clinic;

namespace VetClinic.Data.Data.CMS
{
    public class RecentNews
    {
        [Key]
        public int RecentNewsID { get; set; }

        [Required(ErrorMessage = "Wpisz tytuł linku")]
        [MaxLength(44, ErrorMessage = "Tytuł linku moze mieć max 44 znaków")]
        [Display(Name = "Tytuł linku")]
        public string LinkTitle { get; set; }
        [Required(ErrorMessage = "Wpisz tytuł aktualności")]
        [MaxLength(135, ErrorMessage = "Tytul aktualności moze mieć max 135 znaków")]
        [Display(Name = "Tytuł aktualności")]
        public string Title { get; set; }
        [Display(Name = "Treść")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Wpisz pozycję aktualności")]
        [Display(Name = "Pozycja aktualności")]
        public int Position { get; set; }
        public string Photo { get; set; }
        [Display(Name = "Wybierz zdjęcie")]
        public byte[]? Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int? AddedUserID { get; set; } // użytkownik dodający
        [ForeignKey("AddedUserID")]
        public User RecentNewsAddedUser { get; set; }

        public int? UpdatedUserID { get; set; } // użytkownik modyfikujący
        [ForeignKey("UpdatedUserID")]
        public User RecentNewsUpdatedUser { get; set; }
    }
}
