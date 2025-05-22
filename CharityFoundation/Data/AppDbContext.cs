using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Models;
using Microsoft.AspNetCore.Identity;


namespace CharityFoundation.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Donacija> Donacije { get; set; }
        public DbSet<ZahtjevZaPomoc> ZahtjeviZaPomoc { get; set; }
        public DbSet<Izvjestaj> Izvjestaji { get; set; }
        public DbSet<Notifikacija> Notifikacije { get; set; }
        public DbSet<Akcija> Akcije { get; set; }
        public DbSet<VolonterAkcija> VolonterAkcije { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Identity mora biti prvi

            modelBuilder.Entity<Donacija>()
                .HasOne(d => d.Korisnik)
                .WithMany(u => u.Donacije)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ZahtjevZaPomoc>()
                .HasOne(z => z.Korisnik)
                .WithMany(u => u.Zahtjevi)
                .HasForeignKey(z => z.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Izvjestaj>()
                .HasOne(i => i.Korisnik)
                .WithMany(u => u.Izvjestaji)
                .HasForeignKey(i => i.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notifikacija>()
                .HasOne(n => n.Korisnik)
                .WithMany(u => u.Notifikacije)
                .HasForeignKey(n => n.KorisnikId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VolonterAkcija>()
                .HasOne(va => va.Volonter)
                .WithMany(v => v.Ucesca)
                .HasForeignKey(va => va.VolonterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VolonterAkcija>()
                .HasOne(va => va.Akcija)
                .WithMany(a => a.Ucesnici)
                .HasForeignKey(va => va.AkcijaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
