using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VetClinic.Data.Data.CMS;

namespace VetClinic.Data.Data.Clinic
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public int UserTypeID { get; set; }
        [NotMapped] // Fullname uzywamy tylko do wyswietlania imienia i nazwiska np. w SelectList
        public string Fullname { get { return this.FirstName + " " + this.LastName; } }
        [Required(ErrorMessage = "Imie jest wymagane")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }
        public string HouseNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string Street { get; set; }
        [Required(ErrorMessage = "Miejscowość jest wymagana")]
        public string City { get; set; }
        [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}",
         ErrorMessage = "Kod pocztowy nie jest poprawny. (##-###)")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Email jest wymagany")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
         ErrorMessage = "Adres email nie jest prawidłowy.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Login jest wymagany")]
        [Remote(action: "VerifyLogin", controller: "Employees", AdditionalFields = "UserID")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Password { get; set; } // nie wiem czy potrzebnie, bo bedzie ta autoryzacja .NETowa
        [Required(ErrorMessage = "Numer telefonu jest wymagany")]
        public string Phone { get; set; }
        public string Photo { get; set; }
        public byte[]? Image { get; set; }
        public string CardNumber { get; set; } // karta stałego klienta 
        public bool IsActive { get; set; } // jeżeli nie potwierdzi email to jest nieaktywny lub jeśli został deaktywowany przez admina
        public string ActivationToken { get; set; } // token do aktywacji konta
        public bool AuthorizationEmail { get; set; } // token do aktywacji konta
        public int LoginAttempt { get; set; } // ilosc prób logowanie użytkownika, po x próbach konto się blokuje
        public string Description { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("UserTypeID")]
        public UserType UserType { get; set; }

        [InverseProperty("MedicineAddedUser")]
        public ICollection<Medicine> AddedMedicines { get; set; }
        [InverseProperty("MedicineUpdatedUser")]
        public ICollection<Medicine> UpdatedMedicines { get; set; }

        [InverseProperty("MedicineTypeAddedUser")]
        public ICollection<MedicineType> AddedMedicineTypes { get; set; }
        [InverseProperty("MedicineTypeUpdatedUser")]
        public ICollection<MedicineType> UpdatedMedicineTypes { get; set; }

        [InverseProperty("RecentNewsAddedUser")]
        public virtual ICollection<RecentNews> AddedRecentNews { get; set; }
        [InverseProperty("RecentNewsUpdatedUser")]
        public virtual ICollection<RecentNews> UpdatedRecentNews { get; set; }

        [InverseProperty("PatientUser")]
        public ICollection<Patient> UserPatient { get; set; }
        [InverseProperty("PatientAddedUser")]
        public ICollection<Patient> AddedPatients { get; set; }
        [InverseProperty("PatientUpdatedUser")]
        public ICollection<Patient> UpdatedPatients { get; set; }

        [InverseProperty("PatientTypeAddedUser")]
        public ICollection<PatientType> AddedPatientTypes { get; set; }
        [InverseProperty("PatientTypeUpdatedUser")]
        public ICollection<PatientType> UpdatedPatientTypes { get; set; }

        [InverseProperty("ReservationUser")]
        public ICollection<Reservation> ReservationUser { get; set; }
        [InverseProperty("ReservationAddedUser")]
        public ICollection<Reservation> AddedReservation { get; set; }
        [InverseProperty("ReservationUpdatedUser")]
        public ICollection<Reservation> UpdatedReservation { get; set; }

        [InverseProperty("SenderUser")]
        public ICollection<News> SendersNews { get; set; }
        [InverseProperty("NewsUpdatedUser")]
        public ICollection<News> UpdatedNews { get; set; }

        [InverseProperty("UserTypeAddedUser")]
        public ICollection<UserType> AddedUserTypes { get; set; }
        [InverseProperty("UserTypeUpdatedUser")]
        public ICollection<UserType> UpdatedUserTypes { get; set; }

        [InverseProperty("VetUser")]
        public ICollection<Visit> VetVisits { get; set; }
        [InverseProperty("VisitUser")]
        public ICollection<Visit> UserVisits { get; set; }
        [InverseProperty("VisitAddedUser")]
        public ICollection<Visit> AddedVisits { get; set; }
        [InverseProperty("VisitUpdatedUser")]
        public ICollection<Visit> UpdatedVisits { get; set; }

        [InverseProperty("MedicalSpecializationUser")]
        public ICollection<MedicalSpecialization> UserMedicalSpecialization { get; set; }
        [InverseProperty("MedicalSpecializationAddedUser")]
        public ICollection<MedicalSpecialization> AddedMedicalSpecialization { get; set; }
        [InverseProperty("MedicalSpecializationUpdatedUser")]
        public ICollection<MedicalSpecialization> UpdatedMedicalSpecialization { get; set; }

        [InverseProperty("SpecializationAddedUser")]
        public ICollection<Specialization> AddedUserSpecialization { get; set; }
        [InverseProperty("SpecializationUpdatedUser")]
        public ICollection<Specialization> UpdatedUserSpecialization { get; set; }
        [InverseProperty("TreatmentAddedUser")]
        public ICollection<Treatment> AddedTreatments { get; set; }
        [InverseProperty("TreatmentUpdatedUser")]
        public ICollection<Treatment> UpdatedTreatments { get; set; }

        [InverseProperty("PermissionAddedUser")]
        public ICollection<Permission> AddedPermission { get; set; }
        [InverseProperty("PermissionUpdatedUser")]
        public ICollection<Permission> UpdatedPermission { get; set; }

        [InverseProperty("UserTypePermissionAddedUser")]
        public ICollection<UserTypePermission> AddedUserTypePermission { get; set; }
        [InverseProperty("UserTypePermissionUpdatedUser")]
        public ICollection<UserTypePermission> UpdatedUserTypePermission { get; set; }

    }
}
