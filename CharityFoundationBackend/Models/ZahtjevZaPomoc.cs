namespace CharityFoundationBackend.Models
{
    public enum StatusZahtjeva
    {
        Poslan,
        UObradi,
        Odobren,
        Odbijen
    }

    public class ZahtjevZaPomoc
    {
        public int Id { get; set; }
        public int IdKorisnika { get; set; }
        public string Opis { get; set; } = string.Empty; // dodano
        public DateTime Datum { get; set; }              // dodano
        public int Urgentnost { get; set; }
        public StatusZahtjeva Status { get; set; }
    }
}
