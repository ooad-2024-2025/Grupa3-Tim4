namespace CharityFoundationBackend.Models
{
    public class Akcija
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public DateTime Datum { get; set; }
    }
}
