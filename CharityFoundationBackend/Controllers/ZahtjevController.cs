using Microsoft.AspNetCore.Mvc;
using CharityFoundationBackend.Data;
using CharityFoundationBackend.Models;
using System.Linq;

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

        // GET: api/zahtjevi/korisnik/3
        [HttpGet("korisnik/{id}")]
        public IActionResult GetZahtjeviByKorisnikId(int id)
        {
            var zahtjevi = _context.ZahtjeviZaPomoc
                .Where(z => z.IdKorisnika == id)
                .ToList();

            return Ok(zahtjevi);
        }
        [HttpGet]
public IActionResult GetAll()
{
    var zahtjevi = _context.ZahtjeviZaPomoc.ToList();
    return Ok(zahtjevi);
}

    }
}
