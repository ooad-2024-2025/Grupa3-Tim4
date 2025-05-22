using CharityFoundation.Models.Enums;

namespace CharityFoundation.Models
{
    public class Izvjestaj
    {
        public int Id { get; set; }

        public string Mjesec { get; set; } = string.Empty;
        public int Godina { get; set; }

        public int BrojDonacija { get; set; }        // Donator
        public int BrojZahtjeva { get; set; }        // Primalac pomoći
        public int BrojAkcija { get; set; }          // Volonter

        public double UkupnaVrijednost { get; set; } // Samo Donator

        public string KorisnikId { get; set; } = string.Empty;
        public ApplicationUser Korisnik { get; set; } = null!;

        public string Opis { get; set; } = string.Empty;
        public DateTime Datum { get; set; }
    }
}
