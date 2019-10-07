using Microsoft.EntityFrameworkCore;
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
        public DbSet<Aktualnosc> Aktualnosci { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
    }
}
