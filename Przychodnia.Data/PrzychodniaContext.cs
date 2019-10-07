using Microsoft.EntityFrameworkCore;
using Przychodnia.Data.Data.CMS;
using Przychodnia.Data.Data.Przychodnia;

namespace Przychodnia.Data
{
    public class PrzychodniaContext : DbContext
    {
        public PrzychodniaContext(DbContextOptions<PrzychodniaContext> options)
            : base(options)
        {
        }
        public DbSet<Aktualnosc> Aktualnosci { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
    }
}
