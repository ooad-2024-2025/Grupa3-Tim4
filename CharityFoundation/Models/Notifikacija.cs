using CharityFoundation.Models.Enums;

namespace CharityFoundation.Models
{
    public class Notifikacija
    {
        public int Id { get; set; }

        // 🔗 Foreign Key prema Korisnik
        public string KorisnikId { get; set; } = string.Empty;
        public ApplicationUser Korisnik { get; set; } = null!;

        public string Sadrzaj { get; set; } = string.Empty;
        public TipNotifikacije Tip { get; set; }
        public DateTime Datum { get; set; }
    }
}
