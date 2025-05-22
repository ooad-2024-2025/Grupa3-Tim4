using CharityFoundation.Models.Enums;

namespace CharityFoundation.Models
{
    public class VolonterAkcija
    {
        public int Id { get; set; }

        // 🔗 Foreign Key prema Volonter
        public string VolonterId { get; set; }=string.Empty;
        public ApplicationUser Volonter { get; set; } = null!;

        // 🔗 Foreign Key prema Akcija
        public int AkcijaId { get; set; }
        public Akcija Akcija { get; set; } = null!;

        public StatusUcesca StatusUcesca { get; set; }
    }
}
