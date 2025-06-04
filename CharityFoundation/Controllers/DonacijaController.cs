using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IEmailSender _emailSender;

        public DonacijaController(AppDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Forbid();

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
            if (user?.TipKorisnika != "Donator")
                return Forbid();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Donacija donacija)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "Donator")
                return Forbid();

            donacija.KorisnikId = user.Id;
            donacija.Status = Models.Enums.StatusDonacije.NaCekanju;
            donacija.DatumDonacije = DateTime.Now;

            _context.Add(donacija);
            await _context.SaveChangesAsync();

            // ✉️ Slanje email potvrde
            var subject = "Potvrda donacije";
            var message = $@"
                <h3>Poštovani {user.Ime} {user.Prezime},</h3>
                <p>Vaša donacija je uspješno zaprimljena:</p>
                <ul>
                    <li><strong>Iznos:</strong> {donacija.Iznos} KM</li>
                    <li><strong>Vrsta pomoći:</strong> {donacija.VrstaPomoci}</li>
                    <li><strong>Datum:</strong> {donacija.DatumDonacije:dd.MM.yyyy HH:mm}</li>
                </ul>
                <p>Status donacije: <strong>{donacija.Status}</strong></p>
                <hr/>
                <p>Hvala Vam na ukazanom povjerenju.<br/><i>Charity Foundation</i></p>";

            await _emailSender.SendEmailAsync(user.Email!, subject, message);

            TempData["Uspjeh"] = "Donacija je poslata. Dobit ćete potvrdu na email.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var donacija = await _context.Donacije
                .Include(d => d.Korisnik)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donacija == null) return NotFound();

            if (user.TipKorisnika == "Donator" && donacija.KorisnikId != user.Id)
                return Forbid();

            return View(donacija);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var donacija = await _context.Donacije.FindAsync(id);

            if (donacija == null) return NotFound();

            if (user.TipKorisnika == "Donator" && donacija.KorisnikId != user.Id)
                return Forbid();

            return View(donacija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Donacija donacija)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || id != donacija.Id)
                return Forbid();

            var postojeca = await _context.Donacije.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
            if (postojeca == null)
                return NotFound();

            if (user.TipKorisnika == "Donator")
            {
                if (postojeca.KorisnikId != user.Id)
                    return Forbid();

                donacija.KorisnikId = user.Id;
                donacija.Status = postojeca.Status;
            }
            else if (user.TipKorisnika == "Administrator")
            {
                donacija.KorisnikId = postojeca.KorisnikId;
            }

            try
            {
                _context.Update(donacija);
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
            var donacija = await _context.Donacije
                .Include(d => d.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (donacija == null)
                return NotFound();

            if (user.TipKorisnika != "Donator" || donacija.KorisnikId != user.Id)
                return Forbid();

            return View(donacija);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var donacija = await _context.Donacije.FindAsync(id);

            if (donacija == null || user.TipKorisnika != "Donator" || donacija.KorisnikId != user.Id)
                return Forbid();

            _context.Donacije.Remove(donacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
