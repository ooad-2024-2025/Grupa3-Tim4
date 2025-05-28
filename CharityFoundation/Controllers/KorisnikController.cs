using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Models;

namespace CharityFoundation.Controllers
{
    [Authorize]
    public class KorisnikController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public KorisnikController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // ✅ Prikaz svih korisnika (samo Administrator po TipKorisnika)
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.TipKorisnika != "Administrator")
                return Forbid();

            var korisnici = await _userManager.Users.ToListAsync();
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

        // ✅ Spremanje izmjena
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

        // ✅ Prikaz forme za brisanje
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

        // ✅ Forma za dodavanje novog korisnika
        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.TipKorisnika != "Administrator")
                return Forbid();

            return View();
        }

        // ✅ Dodavanje korisnika
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
