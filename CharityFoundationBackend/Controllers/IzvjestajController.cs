using Microsoft.AspNetCore.Mvc;
using CharityFoundationBackend.Data;
using CharityFoundationBackend.Models;

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

        [HttpGet]
        public IActionResult Get() => Ok(_context.Izvjestaji.ToList());
    }
}