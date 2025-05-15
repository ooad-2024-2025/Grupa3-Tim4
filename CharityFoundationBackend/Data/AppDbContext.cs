using Microsoft.EntityFrameworkCore;
using CharityFoundationBackend.Models;

namespace CharityFoundationBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Donacija> Donacije { get; set; }
        public DbSet<ZahtjevZaPomoc> ZahtjeviZaPomoc { get; set; }
        public DbSet<Izvjestaj> Izvjestaji { get; set; }
        public DbSet<Notifikacija> Notifikacije { get; set; }
        public DbSet<Akcija> Akcije { get; set; }
        public DbSet<VolonterAkcija> VolonterAkcije { get; set; }
    }
}
