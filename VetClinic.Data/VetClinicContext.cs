using Microsoft.EntityFrameworkCore;
using VetClinic.Data.Data.Clinic;
using VetClinic.Data.Data.CMS;

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
        public DbSet<PatientType> PatientTypes { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<MedicalSpecialization> MedicalSpecializations { get; set; }
        public DbSet<VisitMedicine> VisitMedicines { get; set; }
        public DbSet<Treatment> Treatments { get; set; }    
        public DbSet<UserTypePermission> UserTypePermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<VisitTreatment> VisitTreatment { get; set; }
        public DbSet<ScheduleBlock> ScheduleBlocks  { get; set; }
        public DbSet<InaccessibleDay> InaccessibleDays  { get; set; }
        public DbSet<NewsReaded> NewsReadeds  { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScheduleBlock>()
                .HasIndex(p => new { p.Time })
                .IsUnique(true);
        }
    }
}
