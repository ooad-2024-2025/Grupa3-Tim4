namespace CharityFoundationBackend.Models
{
    public enum StatusDonacije
    {
        NaCekanju,
        Prihvacena,
        Odbijena,
        Isporucena
    }

    public class Donacija
    {
        public int Id { get; set; }
        public int IdKorisnika { get; set; }
        public double Iznos { get; set; }
        public string VrstaPomoci { get; set; } = string.Empty;
        public StatusDonacije Status { get; set; }
        public DateTime DatumDonacije { get; set; }
    }
}
