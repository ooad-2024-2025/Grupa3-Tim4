using Microsoft.AspNetCore.Mvc;
using CharityFoundationBackend.Data;
using CharityFoundationBackend.Models;
using System.Linq;

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

        // GET: api/donacija/korisnik/3
        [HttpGet("korisnik/{id}")]
        public IActionResult GetByKorisnikId(int id)
        {
            var donacije = _context.Donacije
                .Where(d => d.IdKorisnika == id)
                .ToList();

            return Ok(donacije);
        }
    }
}
