﻿using Microsoft.EntityFrameworkCore;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Data.CMS;
using VetClinic.Data.Data.VetClinic;

namespace VetClinic.Data
{
    public class VetClinicContext : DbContext
    {
        public VetClinicContext(DbContextOptions<VetClinicContext> options)
            : base(options)
        {
        }

        public DbSet<RecentNews> RecentNews { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineType> MedicineTypes { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<PatientType> PatientTypes { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<MedicalSpecialization> MedicalSpecializations { get; set; }
    }
}
