using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Data;
using CharityFoundation.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var admin = await _userManager.GetUserAsync(User);
            if (admin == null || admin.TipKorisnika != "Administrator") return Forbid();

            ViewBag.Korisnici = await _context.Users
                .Where(u => u.TipKorisnika != "Administrator")
                .Select(u => new { u.Id, ImePrezime = u.Ime + " " + u.Prezime + " (" + u.TipKorisnika + ")" })
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string korisnikId)
        {
            var admin = await _userManager.GetUserAsync(User);
            if (admin == null || admin.TipKorisnika != "Administrator") return Forbid();

            ApplicationUser user;

            if (string.IsNullOrEmpty(korisnikId) || korisnikId == "admin")
            {
                user = admin;
            }
            else
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == korisnikId);
                if (user == null) return NotFound();
            }

            var datum = DateTime.Now;
            var mjesec = datum.ToString("MMMM", new CultureInfo("bs-BA"));
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
                var donacije = await _context.Donacije.Where(d => d.KorisnikId == user.Id).ToListAsync();
                izvjestaj.BrojDonacija = donacije.Count;
                izvjestaj.UkupnaVrijednost = donacije.Sum(d => d.Iznos);
                izvjestaj.Opis = $"Izvještaj za Donatora – {donacije.Count} donacija ukupne vrijednosti {izvjestaj.UkupnaVrijednost:F2} KM";
            }
            else if (user.TipKorisnika == "PrimalacPomoci")
            {
                var zahtjevi = await _context.ZahtjeviZaPomoc.Where(z => z.KorisnikId == user.Id).ToListAsync();
                izvjestaj.BrojZahtjeva = zahtjevi.Count;
                izvjestaj.Opis = $"Izvještaj za Primaoca pomoći – {zahtjevi.Count} zahtjeva";
            }
            else if (user.TipKorisnika == "Volonter")
            {
                var akcije = await _context.VolonterAkcije.Where(v => v.VolonterId == user.Id).ToListAsync();
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

            _context.Izvjestaji.Add(izvjestaj);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var izvjestaj = await _context.Izvjestaji
                .Include(i => i.Korisnik)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (izvjestaj == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (user.TipKorisnika != "Administrator" && izvjestaj.KorisnikId != user.Id))
                return Forbid();

            return View(izvjestaj);
        }

        public async Task<IActionResult> Download(int id)
        {
            var izvjestaj = await _context.Izvjestaji
                .Include(i => i.Korisnik)
                .FirstOrDefaultAsync(i => i.Id == id);

            var user = await _userManager.GetUserAsync(User);
            if (user == null || (user.TipKorisnika != "Administrator" && izvjestaj?.KorisnikId != user.Id))
                return Forbid();

            if (izvjestaj == null)
                return NotFound();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Header()
                        .Text("Charity Foundation - Izvještaj")
                        .FontSize(20).Bold().AlignCenter();

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);
                        col.Item().Text($"Korisnik: {izvjestaj.Korisnik.Ime} {izvjestaj.Korisnik.Prezime}");
                        col.Item().Text($"Tip: {izvjestaj.Korisnik.TipKorisnika}");
                        col.Item().Text($"Mjesec: {izvjestaj.Mjesec} {izvjestaj.Godina}");
                        col.Item().Text($"Datum: {izvjestaj.Datum:g}");
                        col.Item().Text($"Opis: {izvjestaj.Opis}");

                        if (izvjestaj.BrojDonacija > 0)
                            col.Item().Text($"Broj donacija: {izvjestaj.BrojDonacija}, Ukupna vrijednost: {izvjestaj.UkupnaVrijednost:F2} KM");

                        if (izvjestaj.BrojZahtjeva > 0)
                            col.Item().Text($"Broj zahtjeva: {izvjestaj.BrojZahtjeva}");

                        if (izvjestaj.BrojAkcija > 0)
                            col.Item().Text($"Broj akcija: {izvjestaj.BrojAkcija}");
                    });

                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Charity Foundation © ").FontSize(10);
                        txt.Span(DateTime.Now.Year.ToString()).FontSize(10);
                    });
                });
            });

            var pdfBytes = document.GeneratePdf();
            var fileName = $"Izvjestaj_{izvjestaj.Korisnik.Ime}_{izvjestaj.Mjesec}_{izvjestaj.Godina}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}
