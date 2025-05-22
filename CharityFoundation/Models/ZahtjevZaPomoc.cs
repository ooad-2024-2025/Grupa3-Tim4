using CharityFoundation.Models.Enums;

namespace CharityFoundation.Models
{
    public class ZahtjevZaPomoc
    {
        public int Id { get; set; }

        // ✅ Jedini validni Foreign Key
        public string KorisnikId { get; set; }=string.Empty;
        public ApplicationUser Korisnik { get; set; } = null!;

        public string Opis { get; set; } = string.Empty;
        public DateTime Datum { get; set; }
        public int Urgentnost { get; set; }
        public StatusZahtjeva Status { get; set; }
    }
}
