using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Data;
using CharityFoundation.Models;
using CharityFoundation.Models.Enums;

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
            if (user == null) return Forbid();

            ViewBag.TipKorisnika = user.TipKorisnika;

            if (user.TipKorisnika == "Administrator")
            {
                var zahtjevi = await _context.ZahtjeviZaPomoc
                    .Include(z => z.Korisnik)
                    .OrderByDescending(z => z.Datum)
                    .ToListAsync();
                return View(zahtjevi);
            }

            if (user.TipKorisnika == "PrimalacPomoci")
            {
                var zahtjevi = await _context.ZahtjeviZaPomoc
                    .Include(z => z.Korisnik)
                    .Where(z => z.KorisnikId == user.Id)
                    .OrderByDescending(z => z.Datum)
                    .ToListAsync();
                return View(zahtjevi);
            }

            return Forbid();
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "PrimalacPomoci")
                return Forbid();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ZahtjevZaPomoc zahtjev)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "PrimalacPomoci")
                return Forbid();

            zahtjev.KorisnikId = user.Id;
            zahtjev.Status = StatusZahtjeva.NaCekanju;
            zahtjev.Datum = DateTime.Now;

            _context.Add(zahtjev);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var zahtjev = await _context.ZahtjeviZaPomoc
                .Include(z => z.Korisnik)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zahtjev == null) return NotFound();

            // Samo primalac može gledati svoj zahtjev, admin sve
            if (user.TipKorisnika == "PrimalacPomoci" && zahtjev.KorisnikId != user.Id)
                return Forbid();

            return View(zahtjev);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var zahtjev = await _context.ZahtjeviZaPomoc.FindAsync(id);

            if (zahtjev == null) return NotFound();

            if (user.TipKorisnika == "PrimalacPomoci" && zahtjev.KorisnikId != user.Id)
                return Forbid();

            return View(zahtjev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ZahtjevZaPomoc zahtjev)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || id != zahtjev.Id) return Forbid();

            var postojeci = await _context.ZahtjeviZaPomoc.AsNoTracking().FirstOrDefaultAsync(z => z.Id == id);
            if (postojeci == null) return NotFound();

            if (user.TipKorisnika == "PrimalacPomoci")
            {
                if (postojeci.KorisnikId != user.Id)
                    return Forbid();

                zahtjev.KorisnikId = user.Id;
                zahtjev.Status = postojeci.Status;
            }
            else if (user.TipKorisnika == "Administrator")
            {
                zahtjev.KorisnikId = postojeci.KorisnikId;
            }
            else
            {
                return Forbid();
            }

            try
            {
                _context.Update(zahtjev);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var zahtjev = await _context.ZahtjeviZaPomoc
                .Include(z => z.Korisnik)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zahtjev == null)
                return NotFound();

            if (user.TipKorisnika == "PrimalacPomoci" && zahtjev.KorisnikId != user.Id)
                return Forbid();

            return View(zahtjev);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var zahtjev = await _context.ZahtjeviZaPomoc.FindAsync(id);

            if (zahtjev == null)
                return NotFound();

            if (user.TipKorisnika == "PrimalacPomoci" && zahtjev.KorisnikId != user.Id)
                return Forbid();

            _context.ZahtjeviZaPomoc.Remove(zahtjev);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
