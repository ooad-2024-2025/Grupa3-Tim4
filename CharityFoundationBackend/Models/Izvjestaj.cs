namespace CharityFoundationBackend.Models
{
    public class Izvjestaj
    {
        public int Id { get; set; }
        public string Mjesec { get; set; }
        public int Godina { get; set; }
        public int BrojDonacija { get; set; }
        public double UkupnaVrijednost { get; set; }
    }
}