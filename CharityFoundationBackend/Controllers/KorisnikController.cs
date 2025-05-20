using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CharityFoundationBackend.Data;
using CharityFoundationBackend.Models;
using CharityFoundationBackend.Services;
using System.Linq;
using System.Threading.Tasks;

namespace CharityFoundationBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KorisnikController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public KorisnikController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
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

        // 🔐 Login sa JWT tokenom
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var korisnik = _context.Korisnici
                .FirstOrDefault(k => k.Email == model.Email && k.Lozinka == model.Lozinka);

            if (korisnik == null)
                return Unauthorized("Pogrešan email ili lozinka.");

            var token = _jwtService.GenerateToken(korisnik);

            return Ok(new
            {
                token,
                korisnik = new
                {
                    korisnik.Id,
                    korisnik.Ime,
                    korisnik.Prezime,
                    korisnik.Email,
                    uloga = korisnik.TipKorisnika.ToString()
                }
            });
        }

        // 👥 Prikaz svih korisnika (TEST only)
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetAllKorisnici()
        {
            var korisnici = _context.Korisnici.ToList();
            return Ok(korisnici);
        }

        // 🛠️ Ažuriranje korisnika (ADMIN)
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateKorisnik(int id, [FromBody] Korisnik korisnik)
        {
            if (id != korisnik.Id)
                return BadRequest();

            var postoji = await _context.Korisnici.AnyAsync(k => k.Id == id);
            if (!postoji)
                return NotFound();

            _context.Entry(korisnik).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        


    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Lozinka { get; set; }
    }
}
