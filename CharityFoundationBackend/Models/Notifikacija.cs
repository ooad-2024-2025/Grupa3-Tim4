namespace CharityFoundationBackend.Models
{
    public enum TipNotifikacije
    {
        Sistemska,
        Donacija,
        Zahtjev,
        Akcija
    }

    public class Notifikacija
    {
        public int Id { get; set; }
        public int IdKorisnika { get; set; }
        public string Sadrzaj { get; set; }
        public TipNotifikacije Tip { get; set; }
        public DateTime Datum { get; set; }
    }
}