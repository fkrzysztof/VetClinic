using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // RecentNews //
            modelBuilder.Entity<RecentNews>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<RecentNews>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");

            // Medicines //
            modelBuilder.Entity<Medicine>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<Medicine>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");

            // MedicineTypes //
            modelBuilder.Entity<MedicineType>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<MedicineType>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");

            // Operations //
            modelBuilder.Entity<Operation>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<Operation>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");

            // Patients //
            modelBuilder.Entity<Patient>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<Patient>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");

            // PatientTypes //
            modelBuilder.Entity<PatientType>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<PatientType>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");

            // Prescriptions //
            modelBuilder.Entity<Prescription>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<Prescription>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");

            // PrescriptionItems //
            modelBuilder.Entity<PrescriptionItem>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<PrescriptionItem>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");

            // Reservations //
            modelBuilder.Entity<Reservation>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<Reservation>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");

            // Statements //
            modelBuilder.Entity<Statement>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<Statement>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");

            // Users //
            modelBuilder.Entity<User>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<User>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");

            // UserTypes //
            modelBuilder.Entity<UserType>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<UserType>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");

            // Visits //
            modelBuilder.Entity<Visit>()
                    .Property(b => b.IsActive)
                    .HasDefaultValue(true);
            modelBuilder.Entity<Visit>()
                    .Property(b => b.AddedDate)
                    .HasDefaultValueSql("getdate()");
        }
    }
}
