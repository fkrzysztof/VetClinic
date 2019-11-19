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
        [MaxLength(20, ErrorMessage = "Tytul linku moze miec max 20 znakow")]
        [Display(Name = "Tytuł linku")]
        public string LinkTitle { get; set; }
        [Required(ErrorMessage = "Wpisz tytul aktualności")]
        [MaxLength(50, ErrorMessage = "Tytul aktualności moze miec max 50 znakow")]
        [Display(Name = "Tytuł aktualności")]
        public string Title { get; set; }
        [Display(Name = "Treść")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Wpisz pozycję aktualności")]
        [Display(Name = "Pozycja aktualności")]
        public int Position { get; set; }
        [Display(Name = "Wybierz zdjęcie")]
        public string Photo { get; set; }
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
