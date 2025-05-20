namespace CharityFoundationBackend.Models
{
    public class Izvjestaj
    {
        public int Id { get; set; }
        public string Mjesec { get; set; } = string.Empty;
        public int Godina { get; set; }
        public int BrojDonacija { get; set; }
        public double UkupnaVrijednost { get; set; }

        public int KorisnikId { get; set; }               // dodano
        public string Opis { get; set; } = string.Empty;  // dodano
        public DateTime Datum { get; set; }               // dodano
    }
}
