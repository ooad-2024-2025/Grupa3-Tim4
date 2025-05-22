using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Data;
using CharityFoundation.Models;

namespace CharityFoundation.Controllers
{
    [Authorize]
    public class ZahtjevZaPomocController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ZahtjevZaPomocController(AppDbContext context, UserManager<ApplicationUser> userManager)
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
                var sviZahtjevi = await _context.ZahtjeviZaPomoc
                    .Include(z => z.Korisnik)
                    .OrderByDescending(z => z.Datum)
                    .ToListAsync();

                return View(sviZahtjevi);
            }

            if (user.TipKorisnika == "PrimalacPomoci")
            {
                var mojiZahtjevi = await _context.ZahtjeviZaPomoc
                    .Include(z => z.Korisnik)
                    .Where(z => z.KorisnikId == user.Id)
                    .OrderByDescending(z => z.Datum)
                    .ToListAsync();

                return View(mojiZahtjevi);
            }

            return Forbid();
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.TipKorisnika != "PrimalacPomoci")
                return Forbid();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ZahtjevZaPomoc zahtjev)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.TipKorisnika != "PrimalacPomoci")
                return Forbid();

            if (ModelState.IsValid)
            {
                zahtjev.KorisnikId = user.Id;
                zahtjev.Status = Models.Enums.StatusZahtjeva.NaCekanju;
                zahtjev.Datum = DateTime.Now;

                _context.Add(zahtjev);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(zahtjev);
        }

        public async Task<IActionResult> Details(int id)
        {
            var zahtjev = await _context.ZahtjeviZaPomoc
                .Include(z => z.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (zahtjev == null)
                return NotFound();

            return View(zahtjev);
        }

        // Opcionalno: Admin može editovati zahtjeve
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var zahtjev = await _context.ZahtjeviZaPomoc.FindAsync(id);

            if (zahtjev == null)
                return NotFound();

            if (user?.TipKorisnika != "Administrator")
                return Forbid();

            return View(zahtjev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ZahtjevZaPomoc zahtjev)
        {
            var user = await _userManager.GetUserAsync(User);

            if (id != zahtjev.Id || user?.TipKorisnika != "Administrator")
                return Forbid();

            if (ModelState.IsValid)
            {
                _context.Update(zahtjev);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(zahtjev);
        }
    }
}
