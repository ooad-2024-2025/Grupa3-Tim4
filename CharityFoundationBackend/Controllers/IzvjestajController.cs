using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CharityFoundationBackend.Data;
using CharityFoundationBackend.Models;
using CharityFoundationBackend.Services;
using System.Linq;
using System.Text;
using System.Security.Claims;

namespace CharityFoundationBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IzvjestajController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IzvjestajController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Svi izvještaji – samo Administrator
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetAll()
        {
            var izvjestaji = _context.Izvjestaji.ToList();
            return Ok(izvjestaji);
        }

        // ✅ Izvještaji po korisniku – korisnik može dobiti SAMO svoje
        [HttpGet("korisnik/{id}")]
        [Authorize]
        public IActionResult GetByKorisnikId(int id)
        {
            var userId = User.FindFirst("id")?.Value;

            if (userId == null || int.Parse(userId) != id)
                return Forbid();

            var izvjestaji = _context.Izvjestaji
                .Where(i => i.KorisnikId == id)
                .ToList();

            return Ok(izvjestaji);
        }

        // ✅ PDF za sve korisnike – samo Administrator
        [HttpGet("pdf")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GenerateAllPdf()
        {
            var izvjestaji = _context.Izvjestaji.ToList();
            var sb = new StringBuilder();

            sb.AppendLine("Izvještaji - Svi korisnici");
            sb.AppendLine("==============================");

            foreach (var i in izvjestaji)
            {
                sb.AppendLine($"Korisnik ID: {i.KorisnikId}");
                sb.AppendLine($"Opis: {i.Opis}");
                sb.AppendLine($"Datum: {i.Datum.ToShortDateString()}");
                sb.AppendLine("------------------------------");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "application/octet-stream", "Izvjestaji_Svi.txt");
        }

        // ✅ PDF samo za jednog korisnika – samo Administrator (ili možeš proširiti za self-download)
        [HttpGet("pdf/korisnik/{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GenerateByUserPdf(int id)
        {
            var izvjestaji = _context.Izvjestaji
                .Where(i => i.KorisnikId == id)
                .ToList();

            if (!izvjestaji.Any())
                return NotFound("Nema izvještaja za korisnika.");

            var sb = new StringBuilder();
            sb.AppendLine($"Izvještaji za korisnika {id}");
            sb.AppendLine("==============================");

            foreach (var i in izvjestaji)
            {
                sb.AppendLine($"Opis: {i.Opis}");
                sb.AppendLine($"Datum: {i.Datum.ToShortDateString()}");
                sb.AppendLine("------------------------------");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "application/octet-stream", $"Izvjestaj_Korisnik_{id}.txt");
        }
    }
}
