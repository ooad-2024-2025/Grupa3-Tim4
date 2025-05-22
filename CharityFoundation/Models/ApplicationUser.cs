using Microsoft.AspNetCore.Identity;

namespace CharityFoundation.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;

        // Tip korisnika (logika u kodu, nije Identity uloga): "Administrator", "Donator", "PrimalacPomoci", "Volonter"
        public string TipKorisnika { get; set; } = string.Empty;

        // 🔗 Navigacije
        public ICollection<Donacija> Donacije { get; set; } = new List<Donacija>();
        public ICollection<ZahtjevZaPomoc> Zahtjevi { get; set; } = new List<ZahtjevZaPomoc>();
        public ICollection<VolonterAkcija> Ucesca { get; set; } = new List<VolonterAkcija>();
        public ICollection<Notifikacija> Notifikacije { get; set; } = new List<Notifikacija>();
        public ICollection<Izvjestaj> Izvjestaji { get; set; } = new List<Izvjestaj>();
    }
}
