using CharityFoundation.Models.Enums;

namespace CharityFoundation.Models
{
    public class Donacija
    {
        public int Id { get; set; }

        // ✅ Jedini validni Foreign Key
        public string KorisnikId { get; set; } = string.Empty;
        public ApplicationUser Korisnik { get; set; } = null!;

        public double Iznos { get; set; }
        public string VrstaPomoci { get; set; } = string.Empty;
        public StatusDonacije Status { get; set; }
        public DateTime DatumDonacije { get; set; }
    }
}
