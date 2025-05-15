namespace CharityFoundationBackend.Models
{
    public enum TipKorisnika
    {
        Donator,
        Volonter,
        Administrator,
        PrimalacPomoci
    }

    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Lozinka { get; set; }
        public TipKorisnika TipKorisnika { get; set; }
    }
}