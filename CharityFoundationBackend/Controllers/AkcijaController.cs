using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharityFoundationBackend.Data;
using CharityFoundationBackend.Models;
using System.Linq;

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

        // GET: api/akcija
        [HttpGet]
        public IActionResult GetAll()
        {
            var akcije = _context.Akcije.ToList();
            return Ok(akcije);
        }

        // GET: api/akcija/korisnik/5
        [HttpGet("korisnik/{id}")]
        public IActionResult GetAkcijeByVolonterId(int id)
        {
            var akcije = _context.VolonterAkcije
                .Include(va => va.Akcija) // Uključi navigaciju
                .Where(va => va.IdVolontera == id)
                .Select(va => va.Akcija) // Vrati samo povezane akcije
                .ToList();

            return Ok(akcije);
        }
    }
}
