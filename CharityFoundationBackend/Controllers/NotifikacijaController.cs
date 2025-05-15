using Microsoft.AspNetCore.Mvc;
using CharityFoundationBackend.Data;
using CharityFoundationBackend.Models;

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

        [HttpGet("{idKorisnika}")]
        public IActionResult Get(int idKorisnika)
        {
            var lista = _context.Notifikacije.Where(n => n.IdKorisnika == idKorisnika).ToList();
            return Ok(lista);
        }
    }
}