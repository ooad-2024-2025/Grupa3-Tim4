using Microsoft.AspNetCore.Mvc;
using CharityFoundationBackend.Data;
using CharityFoundationBackend.Models;

namespace CharityFoundationBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KorisnikController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KorisnikController(AppDbContext context)
        {
            _context = context;
        }

        // 📥 Registracija
        [HttpPost("register")]
        public IActionResult Register([FromBody] Korisnik korisnik)
        {
            if (_context.Korisnici.Any(k => k.Email == korisnik.Email))
                return BadRequest("Korisnik već postoji.");

            _context.Korisnici.Add(korisnik);
            _context.SaveChanges();
            return Ok("Registracija uspješna.");
        }

        // 🔐 Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var korisnik = _context.Korisnici.FirstOrDefault(k => k.Email == model.Email && k.Lozinka == model.Lozinka);
            if (korisnik == null)
                return Unauthorized("Pogrešan email ili lozinka.");

            return Ok(new
            {
                token = "fake-jwt-token",
                tipKorisnika = korisnik.TipKorisnika.ToString().ToLower()
            });
        }

        // 👥 Prikaz svih korisnika (TEST only)
        [HttpGet]
        public IActionResult GetAllKorisnici()
        {
            var korisnici = _context.Korisnici.ToList();
            return Ok(korisnici);
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Lozinka { get; set; }
    }
}
