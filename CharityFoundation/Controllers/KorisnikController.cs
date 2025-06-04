using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Models;
using CharityFoundation.Data;

namespace CharityFoundation.Controllers
{
    [Authorize]
    public class KorisnikController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public KorisnikController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ✅ Prikaz svih korisnika sa mogućnošću filtracije i rang liste donatora
        public async Task<IActionResult> Index(string? tip)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.TipKorisnika != "Administrator")
                return Forbid();

            ViewBag.Tip = tip;

            var korisnici = await _userManager.Users.ToListAsync();

            if (!string.IsNullOrEmpty(tip))
            {
                korisnici = korisnici
                    .Where(k => k.TipKorisnika == tip)
                    .ToList();

                if (tip == "Donator")
                {
                    var donacije = await _context.Donacije
                        .GroupBy(d => d.KorisnikId)
                        .Select(g => new
                        {
                            KorisnikId = g.Key,
                            UkupniIznos = g.Sum(d => d.Iznos)
                        })
                        .ToDictionaryAsync(g => g.KorisnikId, g => g.UkupniIznos);

                    korisnici = korisnici
                        .OrderByDescending(k => donacije.ContainsKey(k.Id) ? donacije[k.Id] : 0)
                        .ToList();

                    ViewBag.Donacije = donacije;
                }
            }

            return View(korisnici);
        }

        // ✅ Detalji korisnika
        public async Task<IActionResult> Details(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.TipKorisnika != "Administrator")
                return Forbid();

            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik == null) return NotFound();

            return View(korisnik);
        }

        // ✅ Forma za uređivanje korisnika
        public async Task<IActionResult> Edit(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.TipKorisnika != "Administrator")
                return Forbid();

            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik == null) return NotFound();

            return View(korisnik);
        }

        // ✅ Spremanje izmjena korisnika
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.TipKorisnika != "Administrator")
                return Forbid();

            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik == null) return NotFound();

            korisnik.Ime = model.Ime;
            korisnik.Prezime = model.Prezime;
            korisnik.Email = model.Email;
            korisnik.UserName = model.Email;
            korisnik.TipKorisnika = model.TipKorisnika;

            var result = await _userManager.UpdateAsync(korisnik);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ Prikaz forme za brisanje korisnika
        public async Task<IActionResult> Delete(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.TipKorisnika != "Administrator")
                return Forbid();

            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik == null) return NotFound();

            return View(korisnik);
        }

        // ✅ Brisanje korisnika
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.TipKorisnika != "Administrator")
                return Forbid();

            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik != null)
                await _userManager.DeleteAsync(korisnik);

            return RedirectToAction(nameof(Index));
        }

        // ✅ Prikaz forme za kreiranje novog korisnika
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.TipKorisnika != "Administrator")
                return Forbid();

            return View();
        }

        // ✅ Dodavanje novog korisnika
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser model, string password)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.TipKorisnika != "Administrator")
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            model.UserName = model.Email;

            var result = await _userManager.CreateAsync(model, password);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
    }
}
