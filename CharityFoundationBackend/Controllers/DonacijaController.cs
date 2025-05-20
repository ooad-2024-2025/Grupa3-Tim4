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
    public class DonacijaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DonacijaController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET /api/donacija/korisnik/{id} – samo Donator za svoje donacije
        [HttpGet("korisnik/{id}")]
        [Authorize(Roles = "Donator")]
        public IActionResult GetByKorisnikId(int id)
        {
            // ⚠️ Osiguraj da korisnik može vidjeti samo svoje donacije
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim == null || int.Parse(userIdClaim) != id)
                return Forbid();

            var donacije = _context.Donacije
                .Where(d => d.IdKorisnika == id)
                .OrderByDescending(d => d.DatumDonacije)
                .ToList();

            return Ok(donacije);
        }

        // ✅ GET /api/donacija/korisnik/{id} – samo Donator za svoje donacije
        [HttpGet("")]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetAll(int id)
        {
            var donacije = _context.Donacije
                .OrderByDescending(d => d.DatumDonacije)
                .ToList();

            return Ok(donacije);
        }

        // ✅ POST /api/donacija – kreira donaciju (samo Donator)
        [HttpPost]
        [Authorize(Roles = "Donator")]
        public async Task<IActionResult> KreirajDonaciju([FromBody] DonacijaCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim == null || int.Parse(userIdClaim) != dto.IdKorisnika)
                return Forbid();

            var novaDonacija = new Donacija
            {
                IdKorisnika = dto.IdKorisnika,
                Iznos = dto.Iznos,
                VrstaPomoci = dto.VrstaPomoci,
                Status = StatusDonacije.NaCekanju,
                DatumDonacije = DateTime.Now
            };

            _context.Donacije.Add(novaDonacija);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByKorisnikId), new { id = dto.IdKorisnika }, novaDonacija);
        }

        // ✅ PUT /api/donacija/{id}/status – mijenja status (samo Administrator)
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AzurirajStatus(int id, [FromBody] StatusUpdateDto dto)
        {
            var donacija = await _context.Donacije.FindAsync(id);
            if (donacija == null)
                return NotFound();

            donacija.Status = (StatusDonacije)dto.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
