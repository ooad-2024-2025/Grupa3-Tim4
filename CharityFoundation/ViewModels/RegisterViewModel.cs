using System.ComponentModel.DataAnnotations;

namespace CharityFoundation.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Ime { get; set; } = string.Empty;

        [Required]
        public string Prezime { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Tip korisnika")]
        [Required]
        public string TipKorisnika { get; set; } = string.Empty;
    }
}
