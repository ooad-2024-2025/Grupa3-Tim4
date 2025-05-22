using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Data;
using CharityFoundation.Models;

namespace CharityFoundation.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class KorisnikController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public KorisnikController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var korisnici = await _userManager.Users.ToListAsync();
            return View(korisnici);
        }

        public async Task<IActionResult> Details(string id)
        {
            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik == null) return NotFound();
            return View(korisnik);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik == null) return NotFound();
            return View(korisnik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser model)
        {
            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik == null) return NotFound();

            korisnik.Ime = model.Ime;
            korisnik.Prezime = model.Prezime;
            korisnik.Email = model.Email;
            korisnik.TipKorisnika = model.TipKorisnika;

            await _userManager.UpdateAsync(korisnik);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik == null) return NotFound();
            return View(korisnik);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik != null)
                await _userManager.DeleteAsync(korisnik);

            return RedirectToAction(nameof(Index));
        }
    }
}
