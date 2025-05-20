using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CharityFoundationBackend.Data;
using Microsoft.EntityFrameworkCore;
using CharityFoundationBackend.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CharityFoundationBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotifikacijaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotifikacijaController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: /api/notifikacija/korisnik/{id} – korisnik može vidjeti samo svoje notifikacije
        [HttpGet("korisnik/{id}")]
        [Authorize]
        public async Task<IActionResult> GetByKorisnikId(int id)
        {
            var userIdClaim = User.FindFirst("id")?.Value;

            if (userIdClaim == null || int.Parse(userIdClaim) != id)
                return Forbid();

            var lista = await _context.Notifikacije
                .Where(n => n.IdKorisnika == id)
                .OrderByDescending(n => n.Datum)
                .ToListAsync();

            return Ok(lista);
        }
    }
}
