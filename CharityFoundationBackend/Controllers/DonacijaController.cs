using Microsoft.AspNetCore.Mvc;
using CharityFoundationBackend.Data;
using CharityFoundationBackend.Models;

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

        [HttpGet]
        public IActionResult Get() => Ok(_context.Donacije.ToList());
    }
}