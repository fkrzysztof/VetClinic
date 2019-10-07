using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetClinic.Data.Data.CMS
{
    public class Aktualnosc
    {
        [Key]
        public int IdAktualnosci { get; set; } // Klucz główny

        [Required(ErrorMessage = "Wpisz tytuł linku")]
        [MaxLength(20, ErrorMessage = "Tytul linku moze miec max 20 znakow")]
        [Display(Name = "Tytuł linku")]
        public string LinkTytul { get; set; }
        [Required(ErrorMessage = "Wpisz tytul aktualności")]
        [MaxLength(50, ErrorMessage = "Tytul aktualności moze miec max 50 znakow")]
        [Display(Name = "Tytuł aktualności")]
        public string Tytul { get; set; }
        [Display(Name = "Treść")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Tekst { get; set; }
        [Required(ErrorMessage = "Wpisz pozycję aktualności")]
        [Display(Name = "Pozycja aktualności")]
        public int Pozycja { get; set; }
        //[Required(ErrorMessage = "Dodaj zdjęcie aktualności")]
        [Display(Name = "Wybierz zdjęcie")]
        public string Zdjecie { get; set; }
        public bool CzyAktywny { get; set; }
        public DateTime DataDodania { get; set; }
        public DateTime? DataModyfikacji { get; set; }
        //public int? IdUzytkownikaDod { get; set; } // użytkownik dodający
        //public int? IdUzytkownikaMod { get; set; } // użytkownik modyfikujący
    }
}
