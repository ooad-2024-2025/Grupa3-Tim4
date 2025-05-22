using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Data;
using CharityFoundation.Models;

namespace CharityFoundation.Controllers
{
    [Authorize]
    public class DonacijaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonacijaController(AppDbContext context, UserManager<ApplicationUser> userManager)
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
                var sveDonacije = await _context.Donacije
                    .Include(d => d.Korisnik)
                    .OrderByDescending(d => d.DatumDonacije)
                    .ToListAsync();

                return View(sveDonacije);
            }

            if (user.TipKorisnika == "Donator")
            {
                var mojeDonacije = await _context.Donacije
                    .Include(d => d.Korisnik)
                    .Where(d => d.KorisnikId == user.Id)
                    .OrderByDescending(d => d.DatumDonacije)
                    .ToListAsync();

                return View(mojeDonacije);
            }

            return Forbid();
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.TipKorisnika != "Donator")
                return Forbid();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Donacija donacija)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.TipKorisnika != "Donator")
                return Forbid();

            if (ModelState.IsValid)
            {
                donacija.KorisnikId = user.Id;
                donacija.Status = Models.Enums.StatusDonacije.NaCekanju;
                donacija.DatumDonacije = DateTime.Now;

                _context.Add(donacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(donacija);
        }

        public async Task<IActionResult> Details(int id)
        {
            var donacija = await _context.Donacije
                .Include(d => d.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (donacija == null)
                return NotFound();

            return View(donacija);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var donacija = await _context.Donacije.FindAsync(id);
            if (donacija == null)
                return NotFound();

            return View(donacija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Donacija donacija)
        {
            if (id != donacija.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(donacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(donacija);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var donacija = await _context.Donacije
                .Include(d => d.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (donacija == null)
                return NotFound();

            return View(donacija);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donacija = await _context.Donacije.FindAsync(id);
            _context.Donacije.Remove(donacija!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
