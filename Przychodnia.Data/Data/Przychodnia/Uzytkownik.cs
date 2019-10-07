using Przychodnia.Data.Data.CMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Przychodnia.Data.Data.Przychodnia
{
    public class Uzytkownik
    {
        [Key]
        public int IdUzytkownika { get; set; } // klucz główny

        [Required(ErrorMessage = "Imie jest wymagane")]
        public string Imie { get; set; }
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string Nazwisko { get; set; }
        public string Ulica { get; set; }
        [Required(ErrorMessage = "Numer Domu jest wymagany")]
        public string NrDomu { get; set; }
        [Required(ErrorMessage = "Miejscowość jest wymagana")]
        public string Miejscowosc { get; set; }
        [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
        public string KodPocztowy { get; set; }
        [Required(ErrorMessage = "Email jest wymagany")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Login jest wymagany")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Haslo { get; set; }
        public string Telefon { get; set; }
        public string Zdjecie { get; set; }
        public string NrKarty { get; set; } // karta stałego klienta 
        public bool CzyAktywny { get; set; } // jeżeli nie potwierdzi email to jest nieaktywny lub został deaktywowany przez admina
        public int ProbaLogowania { get; set; } // ilosc prób logowanie użytkownika, po 3 próbach konto się blokuje Czyaktywny=false
        public string Opis { get; set; }
        public DateTime DataDodania { get; set; }
        public DateTime? DataModyfikacji { get; set; }
    }
}
