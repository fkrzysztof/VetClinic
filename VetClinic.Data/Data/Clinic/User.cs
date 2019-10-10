using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Data.CMS;

namespace VetClinic.Data.Data.VetClinic
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public int UserTypeID { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }
        public string Address { get; set; }  // ulica numer
        [Required(ErrorMessage = "Miejscowość jest wymagana")]
        public string City { get; set; }
        [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Email jest wymagany")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Login jest wymagany")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Password { get; set; } // nie wiem czy potrzebnie, bo bedzie ta autoryzacja .NETowa
        public string Phone { get; set; }
        public string Photo { get; set; }
        public string CardNumber { get; set; } // karta stałego klienta 
        public bool IsActive { get; set; } // jeżeli nie potwierdzi email to jest nieaktywny lub jeśli został deaktywowany przez admina
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

        [InverseProperty("VetUser")]
        public ICollection<Operation> VetOperations { get; set; }
        [InverseProperty("OperationAddedUser")]
        public ICollection<Operation> AddedOperations { get; set; }
        [InverseProperty("OperationUpdatedUser")]
        public ICollection<Operation> UpdateOperations { get; set; }

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

        [InverseProperty("PrescriptionAddedUser")]
        public ICollection<Prescription> AddedPrescription { get; set; }
        [InverseProperty("PrescriptionUpdatedUser")]
        public ICollection<Prescription> UpdatedPrescription { get; set; }

        [InverseProperty("PrescriptionItemAddedUser")]
        public ICollection<PrescriptionItem> AddedPrescriptionItem { get; set; }
        [InverseProperty("PrescriptionItemUpdatedUser")]
        public ICollection<PrescriptionItem> UpdatedPrescriptionItem { get; set; }

        [InverseProperty("VetUser")]
        public ICollection<Reservation> VetReservations { get; set; }
        [InverseProperty("ReservationUser")]
        public ICollection<Reservation> ReservationUser { get; set; }
        [InverseProperty("ReservationAddedUser")]
        public ICollection<Reservation> AddedReservation { get; set; }
        [InverseProperty("ReservationUpdatedUser")]
        public ICollection<Reservation> UpdatedReservation { get; set; }

        [InverseProperty("SenderUser")]
        public ICollection<Statement> SendersStatement { get; set; }
        [InverseProperty("ReceiverUser")]
        public ICollection<Statement> ReceiversStatement { get; set; }
        [InverseProperty("StatementAddedUser")]
        public ICollection<Statement> AddedStatements { get; set; }
        [InverseProperty("StatementUpdatedUser")]
        public ICollection<Statement> UpdatedStatements { get; set; }

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
    }
}
