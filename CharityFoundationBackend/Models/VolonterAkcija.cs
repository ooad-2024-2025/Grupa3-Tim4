namespace CharityFoundationBackend.Models
{
    public enum StatusUcesca
    {
        Prijavljen,
        Potvrdjen,
        Otkazao,
        Prisustvovao
    }

    public class VolonterAkcija
    {
        public int Id { get; set; }
        public int IdVolontera { get; set; }
        public int IdAkcije { get; set; }
        public StatusUcesca StatusUcesca { get; set; }
    }
}