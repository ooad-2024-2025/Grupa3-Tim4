namespace CharityFoundationBackend.DTOs
{
    public class DonacijaCreateDto
    {
        public int IdKorisnika { get; set; }
        public double Iznos { get; set; }
        public string VrstaPomoci { get; set; } = string.Empty; 
    }
}
