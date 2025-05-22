using CharityFoundation.Models.Enums;

namespace CharityFoundation.Models
{
    public class Akcija
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public DateTime Datum { get; set; }

        // Opcionalno: ako želimo znati koji volonteri su uključeni (inverzna veza)
        public ICollection<VolonterAkcija> Ucesnici { get; set; } = new List<VolonterAkcija>();
    }
}
