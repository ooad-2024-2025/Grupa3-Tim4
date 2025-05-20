namespace CharityFoundationBackend.Models
{
    public enum TipKorisnika
    {
        Administrator,
        Donator,
        PrimalacPomoci,
        Volonter
    }

    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Lozinka { get; set; } = string.Empty;
        public TipKorisnika TipKorisnika { get; set; }
    }
}
