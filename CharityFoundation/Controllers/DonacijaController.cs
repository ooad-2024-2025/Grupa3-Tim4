using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Data;
using CharityFoundation.Models;

namespace CharityFoundation.Controllers
{
    [Authorize(Roles = "Administrator,Donator")]
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
    var user = await _userManager.GetUserAsync(User);
    var donacija = await _context.Donacije
        .Include(d => d.Korisnik)
        .FirstOrDefaultAsync(m => m.Id == id);

    if (donacija == null)
        return NotFound();

    // ✅ Donator može samo svoje donacije gledati
    if (user.TipKorisnika == "Donator" && donacija.KorisnikId != user.Id)
        return Forbid();

    // ✅ Administrator može sve vidjeti
    return View(donacija);
}


        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var donacija = await _context.Donacije.FindAsync(id);

            if (donacija == null)
                return NotFound();

            if (user == null || user.TipKorisnika != "Donator" || donacija.KorisnikId != user.Id)
                return Forbid();

            return View(donacija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Donacija donacija)
        {
            var user = await _userManager.GetUserAsync(User);

            if (id != donacija.Id || user == null || user.TipKorisnika != "Donator")
                return Forbid();

            var postojeca = await _context.Donacije.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
            if (postojeca == null || postojeca.KorisnikId != user.Id)
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    donacija.KorisnikId = user.Id;
                    _context.Update(donacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(donacija);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var donacija = await _context.Donacije
                .Include(d => d.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (donacija == null)
                return NotFound();

            if (user == null || user.TipKorisnika != "Donator" || donacija.KorisnikId != user.Id)
                return Forbid();

            return View(donacija);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var donacija = await _context.Donacije.FindAsync(id);

            if (donacija == null || user == null || user.TipKorisnika != "Donator" || donacija.KorisnikId != user.Id)
                return Forbid();

            _context.Donacije.Remove(donacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
