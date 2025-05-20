using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CharityFoundationBackend.Data;
using CharityFoundationBackend.Models;
using CharityFoundationBackend.Services;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CharityFoundationBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AkcijaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AkcijaController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: /api/akcija – dozvoljeno svim ulogama
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var akcije = await _context.Akcije.ToListAsync();
            return Ok(akcije);
        }

        // ✅ GET: /api/akcija/volonter/{id} – samo Volonter može tražiti SVOJE akcije
        [HttpGet("volonter/{id}")]
        [Authorize(Roles = "Volonter")]
        public async Task<IActionResult> GetAkcijeByVolonterId(int id)
        {
            // ⚠️ opcionalno: validacija da korisnik traži svoje akcije (iz tokena)
            var akcije = await _context.VolonterAkcije
                .Include(va => va.Akcija)
                .Where(va => va.IdVolontera == id)
                .Select(va => va.Akcija)
                .ToListAsync();

            return Ok(akcije);
        }

        // ✅ POST: /api/akcija – samo Admin može kreirati
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> KreirajAkciju([FromBody] Akcija dto)
        {
            var nova = new Akcija
            {
                Naziv = dto.Naziv,
                Opis = dto.Opis,
                Datum = DateTime.Now
            };

            _context.Akcije.Add(nova);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = nova.Id }, nova);
        }
    }
}
