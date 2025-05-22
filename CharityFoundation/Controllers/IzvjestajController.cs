using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Data;
using CharityFoundation.Models;

namespace CharityFoundation.Controllers
{
    [Authorize]
    public class IzvjestajController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IzvjestajController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? korisnikId = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Forbid();

            IQueryable<Izvjestaj> query = _context.Izvjestaji.Include(i => i.Korisnik);

            if (user.TipKorisnika == "Administrator")
            {
                if (!string.IsNullOrEmpty(korisnikId))
                    query = query.Where(i => i.KorisnikId == korisnikId);

                var svi = await query.OrderByDescending(i => i.Datum).ToListAsync();
                return View(svi);
            }

            var moji = await query
                .Where(i => i.KorisnikId == user.Id)
                .OrderByDescending(i => i.Datum)
                .ToListAsync();

            return View(moji);
        }

        public async Task<IActionResult> Generisi()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Forbid();

            var datum = DateTime.Now;
            var mjesec = datum.ToString("MMMM");
            var godina = datum.Year;

            var izvjestaj = new Izvjestaj
            {
                KorisnikId = user.Id,
                Mjesec = mjesec,
                Godina = godina,
                Datum = datum
            };

            if (user.TipKorisnika == "Donator")
            {
                var donacije = await _context.Donacije
                    .Where(d => d.KorisnikId == user.Id)
                    .ToListAsync();

                izvjestaj.BrojDonacija = donacije.Count;
                izvjestaj.UkupnaVrijednost = donacije.Sum(d => d.Iznos);
                izvjestaj.Opis = $"Izvještaj za Donatora – {donacije.Count} donacija ukupne vrijednosti {izvjestaj.UkupnaVrijednost} KM";
            }
            else if (user.TipKorisnika == "PrimalacPomoci")
            {
                var zahtjevi = await _context.ZahtjeviZaPomoc
                    .Where(z => z.KorisnikId == user.Id)
                    .ToListAsync();

                izvjestaj.BrojZahtjeva = zahtjevi.Count;
                izvjestaj.Opis = $"Izvještaj za Primaoca pomoći – {zahtjevi.Count} zahtjeva";
            }
            else if (user.TipKorisnika == "Volonter")
            {
                var akcije = await _context.VolonterAkcije
                    .Where(v => v.VolonterId == user.Id)
                    .ToListAsync();

                izvjestaj.BrojAkcija = akcije.Count;
                izvjestaj.Opis = $"Izvještaj za Volontera – učestvovao na {akcije.Count} akcija";
            }
            else if (user.TipKorisnika == "Administrator")
            {
                var sviDonatori = await _context.Donacije.ToListAsync();
                var sviZahtjevi = await _context.ZahtjeviZaPomoc.ToListAsync();
                var sviVolonteri = await _context.VolonterAkcije.ToListAsync();

                izvjestaj.BrojDonacija = sviDonatori.Count;
                izvjestaj.BrojZahtjeva = sviZahtjevi.Count;
                izvjestaj.BrojAkcija = sviVolonteri.Count;
                izvjestaj.UkupnaVrijednost = sviDonatori.Sum(d => d.Iznos);

                izvjestaj.Opis = $"Administratorski izvještaj – {sviDonatori.Count} donacija, {sviZahtjevi.Count} zahtjeva, {sviVolonteri.Count} učešća";
            }
            else
            {
                return Forbid();
            }

            _context.Izvjestaji.Add(izvjestaj);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var izvjestaj = await _context.Izvjestaji
                .Include(i => i.Korisnik)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (izvjestaj == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Forbid();

            if (user.TipKorisnika == "Administrator" || izvjestaj.KorisnikId == user.Id)
                return View(izvjestaj);

            return Forbid();
        }
    }
}
