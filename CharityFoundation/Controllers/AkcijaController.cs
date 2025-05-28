using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CharityFoundation.Data;
using CharityFoundation.Models;

namespace CharityFoundation.Controllers
{
    [Authorize]
    public class AkcijaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AkcijaController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ Prikaz svih akcija za admina i vlastitih akcija za volontera
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Forbid();

            if (user.TipKorisnika == "Administrator")
            {
                var sveAkcije = await _context.Akcije
                    .Include(a => a.Ucesnici).ThenInclude(va => va.Volonter)
                    .OrderByDescending(a => a.Datum)
                    .ToListAsync();

                ViewBag.TipKorisnika = "Administrator";
                return View(sveAkcije);
            }

            if (user.TipKorisnika == "Volonter")
            {
                var mojeAkcije = await _context.VolonterAkcije
                    .Where(va => va.VolonterId == user.Id)
                    .Include(va => va.Akcija)
                    .Select(va => va.Akcija)
                    .OrderByDescending(a => a.Datum)
                    .ToListAsync();

                ViewBag.TipKorisnika = "Volonter";
                return View(mojeAkcije);
            }

            return Forbid();
        }

        // ✅ Kreiranje akcije i dodjela volontera (Admin)
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "Administrator")
                return Forbid();

            ViewBag.Volonteri = await _userManager.Users
                .Where(u => u.TipKorisnika == "Volonter")
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Akcija akcija, string volonterId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "Administrator")
                return Forbid();

            if (ModelState.IsValid)
            {
                _context.Akcije.Add(akcija);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(volonterId))
                {
                    _context.VolonterAkcije.Add(new VolonterAkcija
                    {
                        AkcijaId = akcija.Id,
                        VolonterId = volonterId,
                        StatusUcesca = Models.Enums.StatusUcesca.Prijavljen
                    });
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Volonteri = await _userManager.Users
                .Where(u => u.TipKorisnika == "Volonter")
                .ToListAsync();

            return View(akcija);
        }

        // ✅ Detalji akcije
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Forbid();

            var akcija = await _context.Akcije
                .Include(a => a.Ucesnici).ThenInclude(va => va.Volonter)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (akcija == null)
                return NotFound();

            return View(akcija);
        }

        // ✅ Uređivanje akcije (Admin)
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "Administrator")
                return Forbid();

            var akcija = await _context.Akcije
                .Include(a => a.Ucesnici)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (akcija == null)
                return NotFound();

            ViewBag.Volonteri = await _userManager.Users
                .Where(u => u.TipKorisnika == "Volonter")
                .ToListAsync();

            return View(akcija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Akcija akcija, string volonterId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "Administrator")
                return Forbid();

            if (id != akcija.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(akcija);
                await _context.SaveChangesAsync();

                // zamijeni volontera ako je selektovan
                var postojece = await _context.VolonterAkcije
                    .Where(va => va.AkcijaId == akcija.Id)
                    .ToListAsync();

                _context.VolonterAkcije.RemoveRange(postojece);
                if (!string.IsNullOrEmpty(volonterId))
                {
                    _context.VolonterAkcije.Add(new VolonterAkcija
                    {
                        AkcijaId = akcija.Id,
                        VolonterId = volonterId,
                        StatusUcesca = Models.Enums.StatusUcesca.Prijavljen
                    });
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Volonteri = await _userManager.Users
                .Where(u => u.TipKorisnika == "Volonter")
                .ToListAsync();

            return View(akcija);
        }

        // ✅ Brisanje akcije (Admin)
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "Administrator")
                return Forbid();

            var akcija = await _context.Akcije
                .Include(a => a.Ucesnici).ThenInclude(va => va.Volonter)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (akcija == null)
                return NotFound();

            return View(akcija);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.TipKorisnika != "Administrator")
                return Forbid();

            var akcija = await _context.Akcije.FindAsync(id);
            if (akcija != null)
            {
                var povezani = await _context.VolonterAkcije.Where(va => va.AkcijaId == akcija.Id).ToListAsync();
                _context.VolonterAkcije.RemoveRange(povezani);
                _context.Akcije.Remove(akcija);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
