using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Data;
using CharityFoundation.Models;

namespace CharityFoundation.Controllers
{
    [Authorize]
    public class AkcijaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AkcijaController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Forbid();

            if (user.TipKorisnika == "Administrator")
            {
                var sveAkcije = await _context.Akcije
                    .Include(a => a.Ucesnici)
                    .ThenInclude(va => va.Volonter)
                    .OrderByDescending(a => a.Datum)
                    .ToListAsync();

                return View(sveAkcije);
            }

            if (user.TipKorisnika == "Volonter")
            {
                var mojeAkcije = await _context.VolonterAkcije
                    .Where(va => va.VolonterId == user.Id)
                    .Include(va => va.Akcija)
                    .Select(va => va.Akcija)
                    .OrderByDescending(a => a.Datum)
                    .ToListAsync();

                return View(mojeAkcije);
            }

            return Forbid();
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "Administrator")
                return Forbid();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Akcija akcija)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "Administrator")
                return Forbid();

            if (ModelState.IsValid)
            {
                _context.Akcije.Add(akcija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(akcija);
        }

        // Prikaz akcije sa volonterima
        public async Task<IActionResult> Details(int id)
        {
            var akcija = await _context.Akcije
                .Include(a => a.Ucesnici)
                .ThenInclude(va => va.Volonter)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (akcija == null)
                return NotFound();

            return View(akcija);
        }

        // Dodavanje volontera (samo admin)
        public async Task<IActionResult> DodajVolontera(int akcijaId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "Administrator")
                return Forbid();

            ViewBag.AkcijaId = akcijaId;
            ViewBag.Volonteri = await _userManager.Users
                .Where(u => u.TipKorisnika == "Volonter")
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajVolontera(int akcijaId, string volonterId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "Administrator")
                return Forbid();

            var postoji = await _context.VolonterAkcije
                .AnyAsync(va => va.AkcijaId == akcijaId && va.VolonterId == volonterId);

            if (!postoji)
            {
                _context.VolonterAkcije.Add(new VolonterAkcija
                {
                    AkcijaId = akcijaId,
                    VolonterId = volonterId,
                    StatusUcesca = Models.Enums.StatusUcesca.Prijavljen
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = akcijaId });
        }
    }
}
