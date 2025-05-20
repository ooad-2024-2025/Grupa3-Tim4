using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CharityFoundationBackend.Data;
using CharityFoundationBackend.Models;
using CharityFoundationBackend.DTOs;
using CharityFoundationBackend.Services; 
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace CharityFoundationBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZahtjevController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ZahtjevController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Svi zahtjevi – samo Administrator
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetAll()
        {
            var zahtjevi = _context.ZahtjeviZaPomoc.ToList();
            return Ok(zahtjevi);
        }

        // ✅ Zahtjevi za jednog korisnika – samo vlasnik može pristupiti
        [HttpGet("korisnik/{id}")]
        [Authorize]
        public IActionResult GetZahtjeviByKorisnikId(int id)
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId == null || int.Parse(userId) != id)
                return Forbid();

            var zahtjevi = _context.ZahtjeviZaPomoc
                .Where(z => z.IdKorisnika == id)
                .OrderByDescending(z => z.Datum)
                .ToList();

            return Ok(zahtjevi);
        }

        // ✅ PrimalacPomoci kreira zahtjev – ali samo za sebe
        [HttpPost]
        [Authorize(Roles = "PrimalacPomoci")]
        public async Task<IActionResult> KreirajZahtjev([FromBody] ZahtjevCreateDto dto)
        {
            var userId = User.FindFirst("id")?.Value;
            if (userId == null || int.Parse(userId) != dto.KorisnikId)
                return Forbid();

            var novi = new ZahtjevZaPomoc
            {
                IdKorisnika = dto.KorisnikId,
                Opis = dto.Opis,
                Datum = DateTime.Now,
                Urgentnost = 1, // možeš dopuniti DTO ako treba
                Status = Models.StatusZahtjeva.Poslan
            };

            _context.ZahtjeviZaPomoc.Add(novi);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetZahtjeviByKorisnikId), new { id = dto.KorisnikId }, novi);
        }

        // ✅ Ažuriranje statusa – samo Administrator
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AzurirajStatus(int id, [FromBody] StatusUpdateDto dto)
        {
            var zahtjev = await _context.ZahtjeviZaPomoc.FindAsync(id);
            if (zahtjev == null)
                return NotFound();

            zahtjev.Status = (Models.StatusZahtjeva)dto.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
